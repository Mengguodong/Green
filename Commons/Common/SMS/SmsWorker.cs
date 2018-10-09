using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SmsWorker
    {
        //public String BaseUrl { get; set; }

        //public String id { get; set; }
        //public String MD5_td_code { get; set; }
        //public String corp_service { get; set; }

        //public SmsWorker()
        //{
        //    BaseUrl = "http://cloud.baiwutong.com:8080/post_sms.do?";
        //    id = "wj2533";
        //    MD5_td_code = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("7096wh1069011612533", "MD5");
        //}

        SmsService.SmsWorkerClient Sms = new SmsService.SmsWorkerClient();
        public String Send(string mobile, string msg_content, string corp_msg_id, string ext)
        {
            string gbMsg = System.Web.HttpUtility.UrlEncode(msg_content, System.Text.Encoding.GetEncoding("gbk"));
            //string UTFMsg = System.Web.HttpUtility.UrlEncode(msg_content, System.Text.Encoding.GetEncoding("UTF-8"));
            //构造请求的Url
            //String mUrl = String.Format("{0}id={1}&MD5_td_code={2}&msg_id={3}&mobile={4}&msg_content={5}&ext={6}",
            //                       BaseUrl, id, MD5_td_code, corp_msg_id, mobile, msg_content, ext);
            //return HttpGet(mUrl);

            return Sms.Send(mobile, gbMsg, corp_msg_id, ext);
        }

        //private String HttpGet(String URL)
        //{
        //    string Result = "";
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
        //    //做请求
        //    request.ContentType = "application/x-www-form-urlencoded;charset=gbk";
        //    request.MaximumAutomaticRedirections = 4;
        //    request.MaximumResponseHeadersLength = 4;
        //    request.Method = "post";

        //    request.Credentials = CredentialCache.DefaultCredentials;
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    //读结果
        //    Stream receiveStream = response.GetResponseStream();
        //    StreamReader readStream = new StreamReader(receiveStream, Encoding.GetEncoding("gb2312"));
        //    Result = readStream.ReadToEnd();
        //    response.Close();
        //    readStream.Dispose();

        //    return Result;
        //}
    }
}
