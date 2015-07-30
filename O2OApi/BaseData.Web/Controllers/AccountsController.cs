using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using BaseData.DataAccess;
using BaseData.Model;
using Webdiyer.WebControls.Mvc;

namespace BaseData.Web.Controllers
{
    public class AccountsController : Controller
    {
        private MyDataContext db = new MyDataContext();

        // GET: Accounts
        public ActionResult Index(string key, int id = 1)
        {
            return ajaxSearchGetResult(key, id);
        }

        private ActionResult ajaxSearchGetResult(string key, int id = 1)
        {
            var qry = db.Accounts.AsQueryable();
            if (!String.IsNullOrWhiteSpace(key))
                qry = qry.Where(x => x.AccountNo.Contains(key) || x.Email.Contains(key));
            var model = qry.OrderByDescending(a => a.RegDate).ToPagedList(id, 10);
            if (Request.IsAjaxRequest())
                return PartialView("_AccountSearchGet", model);
            return View(model);
        }

        // GET: Accounts/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
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
