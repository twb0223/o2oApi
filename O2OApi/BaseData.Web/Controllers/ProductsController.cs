using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using BaseData.DataAccess;
using BaseData.Model;
using Jil;
using Webdiyer.WebControls.Mvc;


namespace BaseData.Web.Controllers
{
    [MyActionFilter]
    public class ProductsController : Controller
    {
        private MyDataContext db = new MyDataContext();
        // GET: Products
        public ActionResult Index(string key,int type=-1, int id = 1)
        {
            return ajaxSearchGetResult(key,type, id);
        }
        private ActionResult ajaxSearchGetResult(string key,int type=-1,int id = 1)
        {
            var qry = db.Products.Include(x => x.ProductType).AsQueryable();
            if (type!=-1)
            {
                qry = qry.Where(x => x.ProductTypeID == type);
            }
            if (!String.IsNullOrWhiteSpace(key))
                qry = qry.Where(x => x.ProductCode.Contains(key) || x.ProdcutName.Contains(key));
            var model = qry.OrderByDescending(a => a.ProductCode).ToPagedList(id, 10);
            if (Request.IsAjaxRequest())
                return PartialView("_ProductsSearchGet", model);
            return View(model);
        }
        // GET: Products/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Create
        [HttpPost]
        public async Task<ActionResult> Create(string jsonstr)
        {
            var res = new JsonResult();
            try
            {
                var model = JSON.Deserialize<Product>(jsonstr);
                db.Products.Add(model);
                await db.SaveChangesAsync();
                res.Data = "OK";
            }
            catch (Exception)
            {
                res.Data = "ERROR";
            }
            return res;
        }
        public ActionResult CheckCode(string code)
        {
            var res = new JsonResult();
            var num = db.Products.Count(x => x.ProductCode == code);
            if (num > 0)
            {
                res.Data = "ishave";
            }
            else
            {
                res.Data = "canuse";
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return res;
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> GetForEdit(string id)
        {
            var res = new JsonResult();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product model = await db.Products.FindAsync(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            else
            {
                res.Data = model;
                res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return res;
            }
        }


        [HttpPost]
        public async Task<ActionResult> Edit(string jsonstr)
        {
            var res = new JsonResult();
            if (ModelState.IsValid)
            {
                
                var model = JSON.Deserialize<Product>(jsonstr);
                var entiy = db.Products.Find(model.ProductCode);
                entiy.ProdcutName = model.ProdcutName;
                entiy.ProductCode = model.ProductCode;
                entiy.ProductTypeID = model.ProductTypeID;
                entiy.ProdcutDes = model.ProdcutDes;
                entiy.DynamicPrice = model.DynamicPrice;
                try
                {
                    db.Entry(entiy).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    res.Data = "OK";
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                res.Data = "ERROR";
            }
            return res;
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product model = await db.Products.FindAsync(id);
            var res = new JsonResult();
            if (model == null)
            {
                res.Data = "ERROR";
            }
            else
            {
                db.Products.Remove(model);
                await db.SaveChangesAsync();
                res.Data = "OK";
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return res;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
