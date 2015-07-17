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
using BaseData.Web.ViewModels;
using Newtonsoft.Json;
using BaseData.Web.Common;

namespace BaseData.Web.Controllers
{
    public class UsersController : Controller
    {
        private MyDataContext db = new MyDataContext();
        public ActionResult Index()
        {
            var list = db.Users.Include(x=>x.Area).Include(x=>x.Area.City).ToList();

            return View(list);
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Create(string jsonstr)
        {
            var res = new JsonResult();
            if (ModelState.IsValid)
            {
                var model = JsonConvert.DeserializeObject<User>(jsonstr);
                model.CreateTime = DateTime.Now;
                var pwd = Tools.MD5Encrypt(model.Password);
                model.Password = pwd;
                db.Users.Add(model);
                await db.SaveChangesAsync();
                res.Data = "OK";
            }
            else
            {
                res.Data = "ERROR";
            }
            return res;

        }
        public ActionResult CheckAccount(string account)
        {
            var res = new JsonResult();
            var num = db.Users.Count(x => x.UserAccount == account);
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

        // GET: Projects/Edit/5
        public async Task<ActionResult> GetForEdit(int? id)
        {
            var res = new JsonResult();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User model = await db.Users.FindAsync(id);
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
                var model = JsonConvert.DeserializeObject<User>(jsonstr);
                var entiy = db.Users.Find(model.UserID);
                if (entiy.Password != model.Password)//如果密码和库中一致表明没改，否则加密先。
                {
                   var pwd = Tools.MD5Encrypt(model.Password);
                   model.Password = pwd;
                }
                entiy.UserName = model.UserName;

                entiy.Password = model.Password;
 
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

        public async Task<ActionResult> Lock(int? id)
        {
            var res = new JsonResult();
            if (ModelState.IsValid)
            {
                User user = await db.Users.FindAsync(id);
                if (user.Enable)
                {
                    user.Enable = false;
                }
                else
                {
                    user.Enable = true;
                }

                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                res.Data = "OK";
            }
            else
            {
                res.Data = "ERROR";
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return res;
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User model = await db.Users.FindAsync(id);
            var res = new JsonResult();
            if (model == null)
            {
                res.Data = "ERROR";
            }
            else
            {
                db.Users.Remove(model);
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
