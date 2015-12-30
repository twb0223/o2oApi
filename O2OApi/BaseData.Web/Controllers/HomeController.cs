using System.Linq;
using System.Web.Mvc;
using BaseData.DataAccess;
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
            string account = from["account"];
            string pwd = from["password"];
            var encypwd = Tools.MD5Encrypt(pwd);
            var restult = db.Users.Any(x => x.UserAccount == account && x.Password == encypwd && x.Enable == true);
            if (!restult)
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
            Session.Abandon();
            Session.Clear();
            return Redirect("Login");
        }
    }
}
