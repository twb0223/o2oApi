using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaseData.Web.Controllers
{
    public class HomeController : Controller
    {
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
            if (account != "admin")
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
