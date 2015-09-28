using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using BaseData.DataAccess;
using BaseData.Model;
using Newtonsoft.Json;
using Webdiyer.WebControls.Mvc;

namespace BaseData.Web.Controllers
{
    [MyActionFilter]
    public class DeliveryMenController : Controller
    {
        private MyDataContext db = new MyDataContext();

        public ActionResult Index(string key, int id = 1)
        {
            return ajaxSearchGetResult(key, id);
        }

        private ActionResult ajaxSearchGetResult(string key, int id = 1)
        {
            var qry = db.DeliveryMen.Include(x=>x.Area).AsQueryable();
            if (!String.IsNullOrWhiteSpace(key))
                qry = qry.Where(x => x.DeliveryManName.Contains(key) || x.IDNumber.Contains(key));
            var model = qry.OrderByDescending(a => a.DeliveryManID).ToPagedList(id, 10);
            if (Request.IsAjaxRequest())
                return PartialView("_DeliveryMenSearchGet", model);
            return View(model);
        }


        // GET: DeliveryMen/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryMan deliveryMan = await db.DeliveryMen.FindAsync(id);
            if (deliveryMan == null)
            {
                return HttpNotFound();
            }
            return View(deliveryMan);
        }

        // GET: DeliveryMen/Create
        public ActionResult Create()
        {
            ViewBag.AreaID = new SelectList(db.Areas, "AreaID", "AreaName");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(string jsonstr)
        {
            var res = new JsonResult();
            if (ModelState.IsValid)
            {
                var model = JsonConvert.DeserializeObject<DeliveryMan>(jsonstr);
                db.DeliveryMen.Add(model);
                await db.SaveChangesAsync();
                res.Data = "OK";
            }
            else
            {
                res.Data = "ERROR";
            }
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
            DeliveryMan model = await db.DeliveryMen.FindAsync(id);
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
                var model = JsonConvert.DeserializeObject<DeliveryMan>(jsonstr);
                try
                {
                    db.Entry(model).State = EntityState.Modified;
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



        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryMan model = await db.DeliveryMen.FindAsync(id);
            var res = new JsonResult();
            if (model == null)
            {
                res.Data = "ERROR";
            }
            else
            {
                db.DeliveryMen.Remove(model);
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
