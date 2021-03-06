﻿using System.Linq;
using System.Web.Http;
using BaseData.DataAccess;
using BaseData.Model;

namespace BaseData.Web.Controllers
{
    /// <summary>
    /// 产品类型
    /// </summary>
    public class ProductTypesApiController : ApiController
    {
        private MyDataContext db = new MyDataContext();

        // GET: api/ProductTypesApi
        /// <summary>
        /// 获取商品类型列表
        /// </summary>
        /// <returns></returns>
          [ApiCompression]
        public IQueryable<ProductType> GetProductTypes()
        {
            return db.ProductTypes;
        }

        
        //// GET: api/ProductTypesApi/5
        ///// <summary>
        ///// 获取单个商品
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[ResponseType(typeof(ProductType))]
        //public async Task<IHttpActionResult> GetProductType(int id)
        //{
        //    ProductType productType = await db.ProductTypes.FindAsync(id);
        //    if (productType == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(productType);
        //}

         protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductTypeExists(int id)
        {
            return db.ProductTypes.Count(e => e.ProductTypeID == id) > 0;
        }
    }
}