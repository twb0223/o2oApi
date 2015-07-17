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
    public class InRecordsController : Controller
    {
        private MyDataContext db = new MyDataContext();

        // GET: InRecords
        public async Task<ActionResult> Index(string key, int id = 1)
        {
            return ajaxSearchGetResult(key, id);
        }

        private ActionResult ajaxSearchGetResult(string key, int id = 1)
        {
            var qry = db.InRecords.AsQueryable();
            if (!String.IsNullOrWhiteSpace(key))
                qry = qry.Where(x => x.ProductCode.Contains(key) || x.CreateBy.Contains(key));
            var model = qry.OrderByDescending(a => a.ProductCode).ToPagedList(id, 10);
            if (Request.IsAjaxRequest())
                return PartialView("_ProductsSearchGet", model);
            return View(model);
        }

        // GET: InRecords/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InRecord inRecord = await db.InRecords.FindAsync(id);
            if (inRecord == null)
            {
                return HttpNotFound();
            }
            return View(inRecord);
        }

        // GET: InRecords/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InRecords/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]

        public async Task<ActionResult> Create(string jsonstr)
        {
            var res = new JsonResult();
            if (ModelState.IsValid)
            {
                var model = JsonConvert.DeserializeObject<InRecord>(jsonstr);
                model.CreateAt = DateTime.Now;
                model.InRecordID = Guid.NewGuid().ToString();
                model.CreateBy = Session["account"].ToString();
                db.InRecords.Add(model);
                await db.SaveChangesAsync();
                res.Data = "OK";
            }
            else
            {
                res.Data = "ERROR";
            }
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
            InRecord model = await db.InRecords.FindAsync(id);
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
                var model = JsonConvert.DeserializeObject<InRecord>(jsonstr);
                var entiy = db.InRecords.Find(model.InRecordID);
                entiy.InRecordID = model.InRecordID;
                entiy.ProductCode = model.ProductCode;
                entiy.InPrice = model.InPrice;
                entiy.Num = model.Num;
                entiy.CreateBy = model.CreateBy;
                entiy.CreateAt = model.CreateAt;
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

      

        // GET: InRecords/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InRecord model = await db.InRecords.FindAsync(id);
            var res = new JsonResult();
            if (model == null)
            {
                res.Data = "ERROR";
            }
            else
            {
                db.InRecords.Remove(model);
                await db.SaveChangesAsync();
                res.Data = "OK";
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return res;
        }

        // POST: InRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            InRecord inRecord = await db.InRecords.FindAsync(id);
            db.InRecords.Remove(inRecord);
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
