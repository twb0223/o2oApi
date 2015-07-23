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

            var list = await db.Database.SqlQuery<OrderProductsFullInfo>(sql).ToListAsync();

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

            list.ForEach(x =>
            {
                vm.TotalAmount += x.Num * x.Prices;
            });
            vm.DiscountAmount = order.DiscountAmount;
            vm.PayAmount = order.PayAmount;

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
            try
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
            catch (Exception)
            {
                res.Data = "ERROR";
                throw;
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

        public async Task<ActionResult> Confirm(string id)
        {
            var res = new JsonResult();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                Order order = await db.Orders.FindAsync(id);
                order.Status = 2;

                var dvman = await db.DeliveryMen.FindAsync(order.DeliveryManID);
                dvman.Status = 0;

                var odl = db.OrderDetails.Where(x => x.OrderID == id).ToList();
                //遍历订单明细，并更新库存信息表
                odl.ForEach(x => {
                    var psmodel = db.ProductsStores.FirstOrDefault(t=> t.ProductCode == x.ProductCode);
                    psmodel.TotalSaleNum += x.Num;
                    db.Entry(psmodel).State = EntityState.Modified;
                });

                db.Entry(order).State = EntityState.Modified;
                db.Entry(dvman).State = EntityState.Modified;

                await db.SaveChangesAsync();
                res.Data = "OK";
            }
            catch (Exception)
            {
                res.Data = "ERROR";
                throw;
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return res;
        }

        public async Task<ActionResult> Back(string jsonstr)
        {
            var res = new JsonResult();
            if (jsonstr == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var model = JsonConvert.DeserializeObject<Order>(jsonstr);
                var order = await db.Orders.FindAsync(model.OrderID);

                order.Status = -1;
                order.Content=model.Content;

                var dvman = await db.DeliveryMen.FindAsync(order.DeliveryManID);
                dvman.Status = 0;

                db.Entry(order).State = EntityState.Modified;
                db.Entry(dvman).State = EntityState.Modified;

                await db.SaveChangesAsync();
                res.Data = "OK";
            }
            catch (Exception)
            {
                res.Data = "ERROR";
                throw;
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
