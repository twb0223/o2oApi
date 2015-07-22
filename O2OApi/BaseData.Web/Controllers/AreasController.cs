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

namespace BaseData.Web.Controllers
{
    public class AreasController : Controller
    {
        private MyDataContext db = new MyDataContext();

        // GET: Areas
        public async Task<ActionResult> Index()
        {
            var areas = db.Areas.Include(a => a.City);
            return View(await areas.ToListAsync());
        }

        // GET: Areas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = await db.Areas.FindAsync(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // GET: Areas/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Citys, "CityID", "CityName");
            return View();
        }

        // POST: Areas/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AreaID,AreaName,CityID")] Area area)
        {
            if (ModelState.IsValid)
            {
                db.Areas.Add(area);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(db.Citys, "CityID", "CityName", area.CityID);
            return View(area);
        }

        // GET: Areas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = await db.Areas.FindAsync(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.Citys, "CityID", "CityName", area.CityID);
            return View(area);
        }

        // POST: Areas/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AreaID,AreaName,CityID")] Area area)
        {
            if (ModelState.IsValid)
            {
                db.Entry(area).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.Citys, "CityID", "CityName", area.CityID);
            return View(area);
        }

        // GET: Areas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = await db.Areas.FindAsync(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // POST: Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Area area = await db.Areas.FindAsync(id);
            db.Areas.Remove(area);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
