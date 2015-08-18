using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BaseData.DataAccess;
using BaseData.Model;

namespace BaseData.Web.Controllers
{
    /// <summary>
    /// 商品信息Api
    /// </summary>
    public class ProductsApiController : ApiController
    {
        private MyDataContext db = new MyDataContext();

        // GET: api/ProductsApi
        /// <summary>
        /// 分页获取商品列表
        /// </summary>
        /// <param name="productTypeID">商品类型ID</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <returns></returns>
        //[Route("api/ProductsApi/{productTypeID}/{pageNum}/{pageSize}")]
        public IQueryable<Product> GetProducts(int productTypeID, int pageNum, int pageSize)
        {
            return db.Products.Include(x => x.ProductType).Where(x => x.ProductTypeID == productTypeID).OrderByDescending(x => x.ProductCode).Skip((pageNum - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 获取各类商品中销量最好的前几位商品列表
        /// </summary>
        /// <param name="productTypeID">商品类型ID</param>
        /// <param name="topNum">数量</param>
        /// <returns></returns>
        public IQueryable<ProductsStore> GetSaleProductsByType(int productTypeID, int topNum)
        {
            return db.ProductsStores.Include(t => t.Product).Where(x => x.Product.ProductTypeID == productTypeID).OrderByDescending(x => x.TotalSaleNum).Take(topNum);
        }

        /// <summary>
        /// 获取各类商品中销量最好的前几位商品列表
        /// </summary>
        /// <param name="topNum">数量</param>
        /// <returns></returns>

        public IQueryable<ProductsStore> GetSaleProducts(int topNum)
        {
            return db.ProductsStores.Include(t => t.Product).OrderByDescending(x => x.TotalSaleNum).Take(topNum);
        }

        /// <summary>
        /// 商品搜索
        /// </summary>
        /// <param name="keyword">查询条件关键字</param>
        /// <returns></returns>
        [ApiCompression]
        public IQueryable<ProductsStore> GetProductsBySerarch(string keyword)
        {
            var qry = db.ProductsStores.Include(x => x.Product).Include(x=>x.Product.ProductType).AsQueryable();
            return qry.Where(x => x.Product.ProdcutName.Contains(keyword)||x.ProductCode.Contains(keyword)|x.Product.ProductType.ProductTypeName.Contains(keyword)).OrderByDescending(x=>x.TotalSaleNum).Take(20);
        }

        // GET: api/ProductsApi/5
        /// <summary>
        /// 获取商品明细
        /// </summary>
        /// <param name="id">商品ID</param>
        /// <returns></returns>
        [ResponseType(typeof(Product))]
        [ApiCompression]
        public async Task<IHttpActionResult> GetProduct(string id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(string id)
        {
            return db.Products.Count(e => e.ProductCode == id) > 0;
        }
    }
}