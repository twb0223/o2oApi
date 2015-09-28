using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseData.Web
{
    /// <summary>
    /// 验证页面的合法性
    /// </summary>
    public class MyHttpModule : IHttpModule
    {
        public void Dispose() { }
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += new EventHandler(context_PreRequestHandlerExecute);
        }
        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication ha = (HttpApplication)sender;
            string path = ha.Context.Request.Url.ToString();
            int n = path.ToLower().IndexOf("/");
            if (n == -1) //是否是登录页面，不是登录页面的话则进入{}
            {
                if (ha.Context.Session["user"] == null) //是否Session中有用户名，若是空的话，转向登录页。
                {
                    ha.Context.Response.Redirect("~/Home/Login");
                }
           }
        }

    }
}