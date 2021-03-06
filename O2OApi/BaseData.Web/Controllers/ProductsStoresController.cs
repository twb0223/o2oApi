﻿using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BaseData.DataAccess;
using Webdiyer.WebControls.Mvc;

namespace BaseData.Web.Controllers
{
    [MyActionFilter]
    public class ProductsStoresController : Controller
    {
        private MyDataContext db = new MyDataContext();
        public ActionResult Index(string key, int id = 1)
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
