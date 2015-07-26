using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace BaseData.Web
{
    /// <summary>
    /// Api 响应内容压缩
    /// </summary>
    public class ApiCompressionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 重写响应内容压缩
        /// </summary>
        /// <param name="actContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actContext)
        {
            var content = actContext.Response.Content;
            var bytes = content == null ? null : content.ReadAsByteArrayAsync().Result;

            var acceptEncoding = actContext.Request.Headers.AcceptEncoding.ToString().ToLower();
            if (acceptEncoding.Contains("gzip"))
            {
                var zlibbedContent = bytes == null ? new byte[0] : CompressionHelper.CompressByte(bytes, "gzip");
                actContext.Response.Content = new ByteArrayContent(zlibbedContent);
                actContext.Response.Content.Headers.Remove("Content-Type");
                actContext.Response.Content.Headers.Add("Content-encoding", "gzip");
            }
            else if (acceptEncoding.Contains("deflate"))
            {
                var zlibbedContent = bytes == null ? new byte[0] : CompressionHelper.CompressByte(bytes, "deflate");
                actContext.Response.Content = new ByteArrayContent(zlibbedContent);
                actContext.Response.Content.Headers.Remove("Content-Type");
                actContext.Response.Content.Headers.Add("Content-encoding", "deflate");
            }
            actContext.Response.Content.Headers.Add("Content-Type", "application/json;charset=utf-8");
            base.OnActionExecuted(actContext);

        }
    }

    public class CompressionHelper
    {
        public static byte[] CompressByte(byte[] str, string type)
        {
            if (str == null)
            {
                return null;
            }

            using (var output = new MemoryStream())
            {
                //使用Ionic压缩
                if (type == "gzip")
                {
                    using (var compressor = new Ionic.Zlib.GZipStream(output, Ionic.Zlib.CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestCompression))
                    {
                        compressor.Write(str, 0, str.Length);
                    }
                }
                else if (type == "deflate")
                {
                    using (var compressor = new Ionic.Zlib.DeflateStream(output, Ionic.Zlib.CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestCompression))
                    {
                        compressor.Write(str, 0, str.Length);
                    }
                }



                //using (var compressor = new GZipStream(output, CompressionMode.Compress))
                //{
                //    compressor.Write(str, 0, str.Length);
                //}

                return output.ToArray();
            }
        }
    }
}