using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BaseData.DataAccess;
using BaseData.Model;
using Newtonsoft.Json;
using Webdiyer.WebControls;
using Webdiyer.WebControls.Mvc;

namespace BaseData.Web.Controllers
{
    public class ProductsStoresController : Controller
    {
        private MyDataContext db = new MyDataContext();

        public async Task<ActionResult> Index(string key, int id = 1)
        {
            return ajaxSearchGetResult(key, id);
        }
        private ActionResult ajaxSearchGetResult(string key, int id = 1)
        {
            var qry = db.ProductsStores.Include(x => x.Product).AsQueryable();
            if (!String.IsNullOrWhiteSpace(key))
                qry = qry.Where(x => x.ProductCode.Contains(key) || x.Product.ProdcutName.Contains(key));
            var model = qry.OrderByDescending(a => a.TotalSaleNum).ToPagedList(id, 10);
            if (Request.IsAjaxRequest())
                return PartialView("_ProductsStoreSearchGet", model);
            return View(model);
        }

        // GET: ProductsStores/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsStore productsStore = await db.ProductsStores.FindAsync(id);
            if (productsStore == null)
            {
                return HttpNotFound();
            }
            return View(productsStore);
        }

        // GET: ProductsStores/Create
        public ActionResult Create()
        {
            ViewBag.ProductCode = new SelectList(db.Products, "ProductCode", "ProdcutName");
            return View();
        }

        // POST: ProductsStores/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductsStoreID,ProductCode,TotalInNum,TotalSaleNum,TotalBreakNum")] ProductsStore productsStore)
        {
            if (ModelState.IsValid)
            {
                db.ProductsStores.Add(productsStore);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProductCode = new SelectList(db.Products, "ProductCode", "ProdcutName", productsStore.ProductCode);
            return View(productsStore);
        }

        // GET: ProductsStores/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsStore productsStore = await db.ProductsStores.FindAsync(id);
            if (productsStore == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductCode = new SelectList(db.Products, "ProductCode", "ProdcutName", productsStore.ProductCode);
            return View(productsStore);
        }

        // POST: ProductsStores/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductsStoreID,ProductCode,TotalInNum,TotalSaleNum,TotalBreakNum")] ProductsStore productsStore)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productsStore).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCode = new SelectList(db.Products, "ProductCode", "ProdcutName", productsStore.ProductCode);
            return View(productsStore);
        }

        // GET: ProductsStores/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsStore productsStore = await db.ProductsStores.FindAsync(id);
            if (productsStore == null)
            {
                return HttpNotFound();
            }
            return View(productsStore);
        }

        // POST: ProductsStores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            ProductsStore productsStore = await db.ProductsStores.FindAsync(id);
            db.ProductsStores.Remove(productsStore);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
