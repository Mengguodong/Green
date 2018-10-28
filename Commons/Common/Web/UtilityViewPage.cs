using System.Web.Mvc;

namespace Common.Web
{
    /// <summary>
    /// ViewPage重写类
    /// 创建人：
    /// 创建时间：2014-10-20
    /// </summary>
    /// <typeparam name="TModel">TModel=>Model</typeparam>
    public class UtilityViewPage<TModel> : WebViewPage<TModel>
    {
        #region 私有成员

        private SND_LoginUserInfo _CurrentUser = null;

        #endregion

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public SND_LoginUserInfo CurrentUser
        {
            get
            {
                if (_CurrentUser == null)
                    _CurrentUser = ViewData["_CurrentUser"] as SND_LoginUserInfo;
                return _CurrentUser;
            }
        }

       
        public override void Execute()
        {
        }
    }

    public class UtilityViewPage : UtilityViewPage<object>
    {
    }
}
