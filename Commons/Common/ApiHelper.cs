using Common.Coms;
using Common.Encrypt;
using Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// WebApi 帮助类
    /// 2015年9月23日
    /// </summary>
    public static class ApiHelper
    {
        /// <summary>
        /// 获取接口数据 
        /// 2016年4月25日 10:44:19
        /// 
        /// </summary>
        /// <typeparam name="T">请求对象</typeparam>
        /// <param name="t">请求对象</param>
        /// <param name="apiurl">接口地址枚举</param>
        /// <param name="method">请求方法</param>
        /// <returns></returns>
        public static ResultModel<T> Get<T>(object obj, EnumPostUrl apiurl, string method)
        {
            string postData = string.Empty;
            if (obj.GetType().ToString().Equals("System.String"))
            {
                postData = obj.ToString();
            }
            else
            {
                postData = JsonConvert.SerializeObject(obj);
            }
            if (ParseHelper.ToInt(GetConfig.GetConfigString("IsOpenInterceptSwith"), 1) == 1)
            {
                //开启加密
                postData = "{\"data\":" + "\"" + AES.AesEncrypt(postData) + "\"}";
            }
            string s = HttpClientHelper.PostResponse(GetApiUrl(apiurl, method), postData);


            return JsonConvert.DeserializeObject<ResultModel<T>>(s == null ? "" : s);
        }
        /// <summary>
        /// 获取响应信息
        /// </summary>
        /// <param name="obj">需要转换成json的数据</param>
        /// <param name="code">返回的状态值</param>
        /// <param name="msg">返回的信息</param>
        /// <returns>返回自定义返回类型</returns>
        public static HttpResponseMessage GetResponseMessage(ApiResponseType type, string massage, object obj)
        {
            string strJson = JsonConvert.SerializeObject(new { code = type, msg = massage, content = obj });
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(strJson, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
        public static HttpResponseMessage GetResponseMessage(object obj)
        {
            string strJson = JsonConvert.SerializeObject(obj);
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(strJson, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
        /// <summary>
        /// 获取客户端版本号
        /// 胡鹏鹏
        /// 2016-1-14 16:34:01
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string ApiVersion(HttpRequestMessage request)
        {
            var contentType = request.Content.Headers.ContentType.ToString();
            var version = System.Text.RegularExpressions.Regex.Match(contentType, "(?<=version=)(.*)").Value;
            return version;
        }

        /// <summary>
        /// 根据stirng版本号返回int版本号
        /// </summary>
        /// <param name="appv"></param>
        /// <returns></returns>
        public static int ApiVersionCode(string appv)
        {
            switch (appv)
            {
                case "2.3":  //Ios
                    return 23;
                case "2.0.0.3":   //Andriod
                    return 23;
                case "2.4":
                    return 24;
                case "2.0.0.4":
                    return 23;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 获取接口请求地址
        /// 2016年4月25日 11:51:45
        /// 窦海超 
        /// </summary>
        /// <param name="apiurl"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private static string GetApiUrl(EnumPostUrl apiurl, string method)
        {
            string strUrl = apiurl.ToString();
            string returnUrl = string.Empty;
            if (strUrl.Equals(EnumPostUrl.advertisement.ToString()))
            {
                returnUrl = GetConfig.GetConfigString("AdApiUrl");
            }
            else if (strUrl.Equals(EnumPostUrl.tool.ToString()))
            {
                returnUrl = GetConfig.GetConfigString("ToolApiUrl");
            }
            else if (strUrl.Equals(EnumPostUrl.order.ToString()))
            {
                returnUrl = GetConfig.GetConfigString("javaorder.webapi");
            }
            else if (strUrl.Equals(EnumPostUrl.goods.ToString()))
            {
                returnUrl = GetConfig.GetConfigString("javagoods.webapi");
            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                return string.Empty;
            }
            return string.Concat(returnUrl, method);
        }


        /// <summary>
        /// Get资源调度  用于获取数据较大的方法
        /// xugy 2015年11月7日17:43:09
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        //public static string HttpGetSchedule(string url, FormUrlEncodedContent content)
        //{
        //    using (var client = content == null ? new HttpClient() : new HttpClient(new HttpClientHandler()))
        //    {
        //        var bytes = client.GetAsync(url).Result.Content.ReadAsByteArrayAsync().Result;
        //        string ss = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
        //        return System.Text.Encoding.UTF8.GetString(DeflateCompressionAttribute.DecompressionByte(bytes));
        //    }
        //}

        /// <summary>
        /// 客户端方法资源调度
        /// xugy 2015年7月10日
        /// </summary>
        /// <param name="url">请求资源url</param>
        /// <param name="content">http上下文</param>
        /// <param name="type">资源调度方式</param>
        /// <returns>结果</returns>
        public static string ProcessMethodSchedule(string url, FormUrlEncodedContent content, AsyncType type)
        {
            string str = null;
            using (var client = content == null ? new HttpClient() : new HttpClient(new HttpClientHandler()))
            {
                switch (type)
                {
                    case AsyncType.GetAsync:
                        str = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                        break;
                    case AsyncType.PostAsync:
                        str = client.PostAsync(url, content).Result.Content.ReadAsStringAsync().Result;
                        break;
                    case AsyncType.PutAsync:
                        str = client.PutAsync(url, content).Result.Content.ReadAsStringAsync().Result;
                        break;
                    case AsyncType.DeleteAsync:
                        str = client.DeleteAsync(url).Result.Content.ReadAsStringAsync().Result;
                        break;
                }
            }
            return str;
        }
    }

    /// <summary>
    /// Api服务器向用户返回的状态码和提示信息
    /// </summary>
    public enum ApiResponseType
    {
        //常用的状态码
        OK = 200,                         //[GET] 服务器成功返回用户请求的数据
        //Created = 201,                    //[POST/PUT/PATCH] 用户新建或修改数据成功。
        //Accepted = 202,                   //表示一个请求已经进入后台排队（异步任务）
        NoContent = 204,                  //指示已成功处理请求并且响应已被设定为无内容
        InvalidRequest = 400,             //[POST/PUT/PATCH]：用户发出的请求有错误，服务器没有进行新建或修改数据的操作
        Unauthorized = 401,               //表示验证失败 （如用户名 密码 错误等）
        Forbidden = 403,                  //表示用户得到授权（与401错误相对），但是访问是被禁止的
        NotFound = 404,                   //用户发出的请求针对的是不存在的记录，服务器没有进行操作
        //NotAcceptable = 406,              //[GET] 用户请求的格式不可得（比如用户请求JSON格式，但是只有XML格式）
        //Gone = 410,                       //[GET] 用户请求的资源被永久删除，且不会再得到的。
        //UnprocesableEntity = 422,         //[POST/PUT/PATCH] 当创建一个对象时，发生一个验证错误
        InternalServerError = 500,        //服务器发生错误，用户将无法判断发出的请求是否成功 

        //自定义状态码（自定义的状态码从1000开始）
        FirestCode = 1001,                //摇一摇奖项为一等奖的标示
        SecondCode = 1002,                //摇一摇奖项为二等奖的标示
        ThirdCode = 1003,                 //摇一摇奖项为三等奖的标示
        NoWinningCode = 1004,             //摇一摇未中奖标示
        NoPrizeTimesCode = 1005,          //摇一摇没有抽奖的测试标示

        OrderProcessFail = 1111,          //订单处理失败状态
        PayProcessFail = 2222,            //支付处理失败状态
        ShoppingCartProcessFail = 3333    //进货单处理失败状态    
    }

    /// <summary>
    /// 返回的实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T>
    {
        public int code { get; set; }
        public string msg { get; set; }
        public T data { get; set; }
    }
    /// <summary>
    /// xugy 客户端资源调度方式
    /// </summary>
    public enum AsyncType
    {
        GetAsync,
        PostAsync,
        PutAsync,
        DeleteAsync
    }
}
