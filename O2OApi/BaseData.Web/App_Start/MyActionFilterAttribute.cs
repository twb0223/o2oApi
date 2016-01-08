using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaseData.Web
{
    public class MyActionFilterAttribute : ActionFilterAttribute
    {
        private bool flag = true;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["account"] == null)
            {
                flag = false;
                filterContext.HttpContext.Response.Redirect("~/Home/Login");
            }
            else
            {
                flag = true;
            }
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 重写action执行完成后 设置响应头
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!flag)
            {
                return;
            }
            HttpRequestBase request = filterContext.HttpContext.Request;
            HttpResponseBase response = filterContext.HttpContext.Response;
            string acceptEncoding = request.Headers["Accept-Encoding"];
            if (acceptEncoding == null)
                return;

            if (acceptEncoding.ToLower().Contains("gzip"))
            {
                response.Filter = new Ionic.Zlib.GZipStream(response.Filter, Ionic.Zlib.CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestCompression);
                response.AppendHeader("Content-Encoding", "gzip");
            }
            else if (acceptEncoding.ToLower().Contains("deflate"))
            {
                response.Filter = new Ionic.Zlib.DeflateStream(response.Filter, Ionic.Zlib.CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestCompression);
                response.AppendHeader("Content-Encoding", "deflate");
            }
            response.Headers.Add("Content-Type", "application/json;charset=utf-8");
        }
    }
}