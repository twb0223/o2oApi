using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BaseData.DataAccess;
using BaseData.Model;

namespace BaseData.Web.Controllers
{
    public class ProductTypesApiController : ApiController
    {
        private MyDataContext db = new MyDataContext();

        // GET: api/ProductTypesApi
        public IQueryable<ProductType> GetProductTypes()
        {
            return db.ProductTypes;
        }

        // GET: api/ProductTypesApi/5
        [ResponseType(typeof(ProductType))]
        public async Task<IHttpActionResult> GetProductType(int id)
        {
            ProductType productType = await db.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }

            return Ok(productType);
        }

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