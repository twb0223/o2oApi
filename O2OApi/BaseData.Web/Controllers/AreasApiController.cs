using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BaseData.DataAccess;
using BaseData.Model;

namespace BaseData.Web.Controllers
{
    /// <summary>
    /// 区域api
    /// </summary>
    public class AreasApiController : ApiController
    {
        private MyDataContext db = new MyDataContext();

        // GET: api/AreasApi
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <returns></returns>
        [ApiCompression]
        public IQueryable<Area> GetAreas()
        {
            return db.Areas.Include(x=>x.City);
        }

        // GET: api/AreasApi/5
        /// <summary>
        /// 获取单个区域
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Area))]
        [ApiCompression]
        public async Task<IHttpActionResult> GetArea(int id)
        {
            Area area = await db.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }

            return Ok(area);
        }

        // PUT: api/AreasApi/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutArea(int id, Area area)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != area.AreaID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(area).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AreaExists(id))
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

        //// POST: api/AreasApi
        //[ResponseType(typeof(Area))]
        //public async Task<IHttpActionResult> PostArea(Area area)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Areas.Add(area);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = area.AreaID }, area);
        //}

        //// DELETE: api/AreasApi/5
        //[ResponseType(typeof(Area))]
        //public async Task<IHttpActionResult> DeleteArea(int id)
        //{
        //    Area area = await db.Areas.FindAsync(id);
        //    if (area == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Areas.Remove(area);
        //    await db.SaveChangesAsync();

        //    return Ok(area);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AreaExists(int id)
        {
            return db.Areas.Count(e => e.AreaID == id) > 0;
        }
    }
}