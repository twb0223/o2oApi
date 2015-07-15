﻿using System;
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
    public class ProductsController : Controller
    {
        private MyDataContext db = new MyDataContext();

        // GET: Products
        public async Task<ActionResult> Index(string key, int id = 1)
        {
            return ajaxSearchGetResult(key, id);
        }
        private ActionResult ajaxSearchGetResult(string key, int id = 1)
        {
            var qry = db.Products.Include(x=>x.ProductType).AsQueryable();
            if (!String.IsNullOrWhiteSpace(key))
                qry = qry.Where(x => x.ProductCode.Contains(key) || x.ProdcutName.Contains(key) || x.ProductType.ProductTypeName.Contains(key));
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
            if (ModelState.IsValid)
            {
                var model = JsonConvert.DeserializeObject<Product>(jsonstr);
                db.Products.Add(model);
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
                var model = JsonConvert.DeserializeObject<Product>(jsonstr);
                var entiy = db.Products.Find(model.ProductCode);
                entiy.ProdcutName = model.ProdcutName;
                entiy.ProductCode = model.ProductCode;
                entiy.ProductTypeID = model.ProductTypeID;
                entiy.ProdcutDes = model.ProdcutDes;
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