using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Common.Web
{
    /// <summary>
    ///     用户登录服务上下文
    ///     创建人：mengguodong
    ///     创建时间：2014-10-20
    /// </summary>
    public class ServiceContext
    {
        private static readonly string _CookieName = PubConstant.COOKIE_NAME;
        // Auxiliary.ConfigKey("cookie_name"); //PubConstant.COOKIE_NAME;//Auxiliary.ConfigKey("cookie_name");

       // private static readonly string _ShopsCookie = PubConstant.SHOPS_COOKIE;
        // Auxiliary.ConfigKey("shops_cookie"); //PubConstant.COOKIE_NAME;//Auxiliary.ConfigKey("cookie_name");

        private string _Value;

        /// <summary>
        ///     登录COOKIE值
        /// </summary>
        public string Value
        {
            get
            {
                if (_Value == null)
                {
                    var cookie = CookieTool.GetCookie(_CookieName); //HttpContext.Current.Request.Cookies[_CookieName];
                    if (!string.IsNullOrWhiteSpace(cookie))
                    {
                        return cookie;
                    }
                    return null;
                }
                return _Value;
            }
        }


        ///// <summary>
        ///// 登录信息【redis存储】
        ///// 创建人：menggd
        ///// 创建时间：2015年11月17日 17:06:13
        ///// </summary>
        //public LoginUserInfo _CurrentUser
        //{
        //    get
        //    {
        //        var value = Value;
        //        if (!string.IsNullOrWhiteSpace(value))
        //        {
        //            LoginUserInfo userinfo = null;
        //            try
        //            {
        //                //解密cookie内容
        //                var userId = BaseRC4.RC4Decrypt(value);
        //                //反序列化当前登录对象
        //                userinfo = new RedisHelper().GetValue<LoginUserInfo>("login:user:" + userId);
        //                //处理老cookie问题
        //                if (userinfo == null)
        //                    CookieTool.RemoveCookie(_CookieName);
        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.WriteInfo(this.GetType(), ex.Message);
        //                userinfo = null;
        //            }

        //            return userinfo;
        //        }

        //        return null;
        //    }
        //}

        ///// <summary>
        ///// 超市登录信息【cookie存储】
        ///// 创建人：menggd
        ///// 创建时间：2015年11月17日 17:07:01
        ///// </summary>
        //public LoginUserInfo CurrentUser
        //{
        //    get
        //    {
        //        var value = Value;
        //        if (!string.IsNullOrWhiteSpace(value))
        //        {
        //            LoginUserInfo userinfo = null;
        //            try
        //            {
        //                //反序列化当前登录对象
        //                userinfo = JsonConvert.DeserializeObject<LoginUserInfo>(value);
        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.WriteInfo(this.GetType(), ex.Message);
        //                userinfo = null;
        //            }

        //            return userinfo;
        //        }

        //        return null;
        //    }
        //}

        /// <summary>
        /// 三农盾
        /// </summary>
        public SND_LoginUserInfo SND_CurrentUser
        {
            get
            {
                var value = Value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    SND_LoginUserInfo userinfo = null;
                    try
                    {
                        //反序列化当前登录对象
                        userinfo = JsonConvert.DeserializeObject<SND_LoginUserInfo>(value);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteInfo(this.GetType(), ex.Message);
                        userinfo = null;
                    }

                    return userinfo;
                }

                return null;
            }
        }



        /// <summary>
        /// 市场后台用户信息【cookie存储】
        /// 创建人：menggd
        /// 创建时间：2015年11月17日 17:07:01
        /// </summary>
        public LoginMarketUserInfo MarketCurrentUser
        {
            get
            {
                var value = Value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    LoginMarketUserInfo userinfo = null;
                    try
                    {
                        //反序列化当前登录对象
                        userinfo = JsonConvert.DeserializeObject<LoginMarketUserInfo>(value);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteInfo(this.GetType(), ex.Message);
                        userinfo = null;
                    }

                    return userinfo;
                }

                return null;
            }
        }

        //public List<int> CurrentShops
        //{
        //    get
        //    {
        //        var value = ShopsValue;
        //        if (!string.IsNullOrWhiteSpace(value))
        //        {
        //            List<int> shops = null;
        //            //解密cookie内容
        //            var data = BaseRC4.RC4Decrypt(value);
        //            //反序列化当前登录对象
        //            shops = JsonConvert.DeserializeObject<List<int>>(data);
        //            return shops;
        //        }
        //        return null;
        //    }
        //}

        /// <summary>
        ///     重置
        /// </summary>
        public void Refresh()
        {
            CookieTool.RemoveCookie(_CookieName);
        }
    }
}