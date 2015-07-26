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
    public class BreakRecordsController : Controller
    {
        private MyDataContext db = new MyDataContext();

        [MvcCompression]
        public ActionResult Index(string key, int id = 1)
        {
            return ajaxSearchGetResult(key, id);
        }

        private ActionResult ajaxSearchGetResult(string key, int id = 1)
        {
            var qry = db.BreakRecords.Include(x => x.Product).AsQueryable();
            if (!String.IsNullOrWhiteSpace(key))
                qry = qry.Where(x => x.ProductCode.Contains(key) || x.CreateBy.Contains(key) || x.Product.ProdcutName.Contains(key));
            var model = qry.OrderByDescending(a => a.ProductCode).ToPagedList(id, 10);
            if (Request.IsAjaxRequest())
                return PartialView("_BreakRecordsSearchGet", model);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(string jsonstr)
        {
            var res = new JsonResult();
            try
            {
                var model = JsonConvert.DeserializeObject<BreakRecord>(jsonstr);
                model.CreateAt = DateTime.Now;
                model.BreakRecordID = Guid.NewGuid().ToString();
                model.CreateBy = Session["account"].ToString();
                db.BreakRecords.Add(model);

                //todo：更新ProductsStore
                var psmodel = db.ProductsStores.FirstOrDefault(x => x.ProductCode == model.ProductCode);
                if (psmodel == null)//首次添加
                {
                    ProductsStore vm = new ProductsStore();
                    vm.ProductCode = model.ProductCode;
                    vm.TotalInNum = model.Num;
                    vm.TotalSaleNum = 0;
                    vm.TotalBreakNum = 0;
                    db.ProductsStores.Add(vm);
                }
                else//添加库存
                {
                    psmodel.TotalBreakNum += model.Num;
                    db.Entry(psmodel).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
                res.Data = "OK";
            }
            catch (Exception)
            {
                res.Data = "ERROR";
                throw;
            }
            return res;
        }
        public async Task<ActionResult> GetForEdit(string id)
        {
            var res = new JsonResult();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BreakRecord model = await db.BreakRecords.FindAsync(id);
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
            try
            {
                var model = JsonConvert.DeserializeObject<BreakRecord>(jsonstr);
                var entiy = db.BreakRecords.Find(model.BreakRecordID);

                var psmodel = db.ProductsStores.FirstOrDefault(x => x.ProductCode == model.ProductCode);
                //重新计算货损总数
                psmodel.TotalBreakNum = psmodel.TotalBreakNum - entiy.Num + model.Num;

                entiy.BreakRecordID = model.BreakRecordID;
                entiy.ProductCode = model.ProductCode;
                entiy.Num = model.Num;
                entiy.CreateBy = model.CreateBy;
                entiy.CreateAt = model.CreateAt;
                db.Entry(entiy).State = EntityState.Modified;
                db.Entry(psmodel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                res.Data = "OK";
            }
            catch (Exception)
            {
                res.Data = "ERROR";
                throw;
            }
            return res;
        }
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BreakRecord model = await db.BreakRecords.FindAsync(id);
            var res = new JsonResult();
            if (model == null)
            {
                res.Data = "ERROR";
            }
            else
            {
                var psmodel = db.ProductsStores.FirstOrDefault(x => x.ProductCode == model.ProductCode);
                //重新计算货损总数
                psmodel.TotalBreakNum = psmodel.TotalBreakNum - model.Num;
                db.Entry(psmodel).State = EntityState.Modified;
                db.BreakRecords.Remove(model);
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
