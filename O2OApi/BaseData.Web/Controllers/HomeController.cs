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
using BaseData.Web.Common;


namespace BaseData.Web.Controllers
{
    public class HomeController : Controller
    {
        private MyDataContext db = new MyDataContext();
        public ActionResult Index()
        {
            ViewBag.Title = "O2O数据接口平台";
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Title = "登录";

            return View();
        }
        public ActionResult CheckUser(FormCollection from)
        {
            //todo:验证账号密码
            string account = from["account"];
            string pwd = from["password"];
            var encypwd = Tools.MD5Encrypt(pwd);
            var restult = db.Users.Any(x => x.UserAccount == account && x.Password == encypwd && x.Enable == true);
            if (restult)
            {
                return Redirect("Login");
            }
            else
            {
                Session["account"] = account;
                return Redirect("index");
            }
        }
        public ActionResult LogOut()
        {
            ViewBag.Title = "登录";
            //todo:注销session;
            Session.Abandon();
            Session.Clear();
            return Redirect("Login");
        }
    }
}
