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
using BaseData.Web.ViewModels;

namespace BaseData.Web.Controllers
{
    public class OrdersController : Controller
    {
        private MyDataContext db = new MyDataContext();

        // GET: Orders
        public async Task<ActionResult> Index(string key, int id = 1)
        {
            return ajaxSearchGetResult(key, id);
        }

        private ActionResult ajaxSearchGetResult(string key, int id = 1)
        {
            var qry = db.Orders.AsQueryable();
            if (!String.IsNullOrWhiteSpace(key))
                qry = qry.Where(x => x.OrderNo.Contains(key) || x.AccountID.Contains(key));
            var model = qry.OrderByDescending(a => a.OrderID).ToPagedList(id, 10);
            if (Request.IsAjaxRequest())
                return PartialView("_OrdersSearchGet", model);
            return View(model);
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            Account acc = await db.Accounts.FindAsync(order.AccountID);
            string sql = string.Format(@"select od.*,p.ProdcutName,p.imgUrl,p.ProductTypeID 
                                        from OrderDetails od inner join [dbo].[Products] p
                                        on od.ProductCode=p.ProductCode where od.OrderID='{0}'", id);

            var list =await db.Database.SqlQuery<OrderProductsFullInfo>(sql).ToListAsync();

            OrderFullInfo vm = new OrderFullInfo();
            vm.OrderID = order.OrderID;
            vm.OrderNo = order.OrderNo;
            vm.AccountID = order.AccountID;
            vm.AccountAddress = acc.DispatchAddress;
            vm.AccountName = acc.ConnectName;
            vm.AccountMobile = acc.Mobile;
            vm.ProductList = list;
            vm.Status = order.Status;
            vm.OrderDate = order.OrderDate;

            list.ForEach(x => {
                vm.TotalPrice += Math.Round(x.Num * x.Prices,2);
            });

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

         [HttpPost]
        public async Task<ActionResult> Send(string jsonstr)
        {
            var res = new JsonResult();
            if (ModelState.IsValid)
            {
                var model = JsonConvert.DeserializeObject<Order>(jsonstr);
                var order = await db.Orders.FindAsync(model.OrderID);
                order.DeliveryManID = model.DeliveryManID;
                order.Status = 1;

                var dvman = await db.DeliveryMen.FindAsync(model.DeliveryManID);
                dvman.Status = 1;

                db.Entry(order).State = EntityState.Modified;
                db.Entry(dvman).State = EntityState.Modified;
                await db.SaveChangesAsync();
                res.Data = "OK";
            }
            else
            {
                res.Data = "ERROR";
            }
            return res;
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
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
