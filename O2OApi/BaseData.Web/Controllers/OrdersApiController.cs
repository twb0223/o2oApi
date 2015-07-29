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
using BaseData.Web.ViewModels;

namespace BaseData.Web.Controllers
{
    /// <summary>
    /// 订单处理
    /// </summary>
    public class OrdersApiController : ApiController
    {
        private MyDataContext db = new MyDataContext();

        // GET: api/OrdersApi
        /// <summary>
        /// 获取所有订单
        /// </summary>
        /// <returns></returns>
        [ApiCompression]
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }
        // GET: api/OrdersApi/5
        /// <summary>
        /// 获取单个订单的订单明细
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        [ResponseType(typeof(OrderFullInfoVM))]
        [ApiCompression]
        [Route("api/OrdersApi/GetByOrder/{OrderID}")]
        public async Task<IHttpActionResult> GetOrderDetails(string OrderID)
        {
            Order order = await db.Orders.Include(x => x.Account).FirstOrDefaultAsync(x => x.OrderID == OrderID);
            var plist = await db.OrderDetails.Include(x => x.Product).Where(x => x.OrderID == order.OrderID)
              .Select(x => new
              {
                  x.OrderDetailsID,
                  x.ProductCode,
                  x.Num,
                  x.Prices,
                  x.Product.ProdcutName,
                  x.Product.ProdcutDes,
                  x.Product.ImgUrl,
                  x.Product.ProductTypeID
              }).ToListAsync();
            OrderFullInfoVM vm = new OrderFullInfoVM();
            vm.OrderID = order.OrderID;
            vm.OrderNo = order.OrderNo;
            vm.AccountID = order.AccountID;
            vm.AccountAddress = order.Account.DispatchAddress;
            vm.AccountName = order.Account.ConnectName;
            vm.AccountMobile = order.Account.Mobile;
            vm.Status = order.Status;
            vm.OrderDate = order.OrderDate;
            vm.DiscountAmount = order.DiscountAmount;
            vm.PayAmount = order.PayAmount;
            vm.ProductList = new List<OrderProductsFullInfoVM>();
            plist.ForEach(x =>
            {
                OrderProductsFullInfoVM opfvm = new OrderProductsFullInfoVM(x.OrderDetailsID, x.ProductCode, x.Num, x.Prices,
                    x.ProdcutName, x.ProdcutDes, x.ImgUrl, x.ProductTypeID);
                vm.ProductList.Add(opfvm);
                vm.TotalAmount += x.Num * x.Prices;
            });

            if (order == null)
            {
                return NotFound();
            }

            return Ok(vm);

        }
        /// <summary>
        /// 获取用户名下所有订单信息
        /// </summary>
        /// <param name="AccountID">用户账号</param>
        /// <returns></returns>
        [ResponseType(typeof(List<OrderFullInfoVM>))]
        [ApiCompression]
        [Route("api/OrdersApi/GetByAccount/{AccountID}")]
        public async Task<IHttpActionResult> GetOrderByAccountID(string AccountID)
        {
            var orderlist = await db.Orders.Include(x => x.Account).Where(x => x.AccountID == AccountID).ToListAsync();
            List<OrderFullInfoVM> list = new List<OrderFullInfoVM>();
            foreach (var order in orderlist)
            {
                var plist = await db.OrderDetails.Include(x => x.Product).Where(x => x.OrderID == order.OrderID)
                            .Select(x => new
                            {
                                x.OrderDetailsID,
                                x.ProductCode,
                                x.Num,
                                x.Prices,
                                x.Product.ProdcutName,
                                x.Product.ProdcutDes,
                                x.Product.ImgUrl,
                                x.Product.ProductTypeID
                            }).ToListAsync();

                OrderFullInfoVM vm = new OrderFullInfoVM();
                vm.OrderID = order.OrderID;
                vm.OrderNo = order.OrderNo;
                vm.AccountID = order.AccountID;
                vm.AccountAddress = order.Account.DispatchAddress;
                vm.AccountName = order.Account.ConnectName;
                vm.AccountMobile = order.Account.Mobile;

                vm.Status = order.Status;
                vm.OrderDate = order.OrderDate;

                vm.ProductList = new List<OrderProductsFullInfoVM>();
                plist.ForEach(x =>
                {
                    OrderProductsFullInfoVM opfvm = new OrderProductsFullInfoVM(x.OrderDetailsID, x.ProductCode, x.Num, x.Prices,
                        x.ProdcutName, x.ProdcutDes, x.ImgUrl, x.ProductTypeID);
                    vm.ProductList.Add(opfvm);
                    vm.TotalAmount += x.Num * x.Prices;
                });
                vm.DiscountAmount = order.DiscountAmount;
                vm.PayAmount = order.PayAmount;
                list.Add(vm);
            }
            if (orderlist == null)
            {
                return NotFound();
            }

            return Ok(list);
        }

        // PUT: api/OrdersApi/5
        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutOrder(string id, Order order)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != order.OrderID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(order).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/OrdersApi
        /// <summary>
        /// 添加新订单
        /// </summary>
        /// <param name="vm">OrderID为空</param>
        /// <returns></returns>
        [ResponseType(typeof(ResultVM))]
        public async Task<IHttpActionResult> PostOrder(PostOrderInfoVM vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Order order = new Order();
            order.OrderID = Guid.NewGuid().ToString().ToUpper();
            order.OrderDate = DateTime.Now;
            order.OrderNo = "O2O" + DateTime.Now.ToString("yyyyMMddhhmmss") + new Random().Next(1000, 9999);
            order.AccountID = vm.AccountID;
            order.PayAmount = vm.PayAmount;
            order.TotalAmount = vm.TotalAmount;
            order.Status = 0;
            order.DiscountAmount = vm.DiscountAmount;
            db.Orders.Add(order);

            foreach (var item in vm.ProductList)
            {
                OrderDetail od = new OrderDetail();
                od.ProductCode = item.ProductCode;
                od.OrderID = order.OrderID;
                od.Num = item.Num;
                od.Prices = item.Prices;
                db.OrderDetails.Add(od);
            }

            ResultVM model = new ResultVM();

            try
            {
                await db.SaveChangesAsync();
                model.Result = "OK";
                model.Message = "订单编号：" + order.OrderNo;
            }
            catch (DbUpdateException ex)
            {
                model.Result = "Error";
                if (OrderExists(order.OrderID))
                {
                    model.Message = "重复提交";
                    return Conflict();
                }
                else
                {
                    model.Message = ex.Message;
                    throw;
                }
            }
            return Ok(model);
        }

        // DELETE: api/OrdersApi/5
        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[ResponseType(typeof(Order))]
        //public async Task<IHttpActionResult> DeleteOrder(string id)
        //{
        //    Order order = await db.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Orders.Remove(order);
        //    await db.SaveChangesAsync();

        //    return Ok(order);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(string id)
        {
            return db.Orders.Count(e => e.OrderID == id) > 0;
        }
    }
}