using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BaseData.DataAccess;
using System.Data.Entity;

namespace BaseData.Web.Controllers
{
    /// <summary>
    /// 商品图片上传
    /// </summary>
    public class UploadApiController : ApiController
    {
        private MyDataContext db = new MyDataContext();

        /// <summary>
        /// 商品图片上传
        /// </summary>
        /// <returns></returns>
        [ApiCompression]
        public async Task<HttpResponseMessage> PostFormData()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context     
            HttpRequestBase request = context.Request;//定义传统request对象

            string pcode = request.Form["code"];//获取传递端产品条码



            // Check if the request contains multipart/form-data.
            // 检查该请求是否含有multipart/form-data
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string root = HttpContext.Current.Server.MapPath("~/UploadFiles");
            // var provider = new MultipartFormDataStreamProvider(root);
            var provider = new RenamingMultipartFormDataStreamProvider(root);
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                string imgurl = "";
                foreach (MultipartFileData file in provider.FileData)
                {
                    imgurl = file.LocalFileName.Replace(root + "\\", "");
                }
                //更新产品对应端图片，以;分割
                var model = await db.Products.FindAsync(pcode);
                model.ImgUrl = imgurl;
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }

        }
    }
    public class RenamingMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public string Root { get; set; }
        public RenamingMultipartFormDataStreamProvider(string root)
            : base(root)
        {
            Root = root;
        }
        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            string filePath = headers.ContentDisposition.FileName;
            // Multipart requests with the file name seem to always include quotes.
            if (filePath.StartsWith(@"""") && filePath.EndsWith(@""""))
                filePath = filePath.Substring(1, filePath.Length - 2);

            var filename = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(filePath);
            var contentType = headers.ContentType.MediaType;
            return filename + extension;
        }

    }
}
