using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BaseData.DataAccess;
using BaseData.Model;

namespace BaseData.Web.Controllers
{
    public class DeliveryMenApiController : ApiController
    {
        private MyDataContext db = new MyDataContext();

        // GET: api/DeliveryMenApi
        public IQueryable<DeliveryMan> GetDeliveryMen()
        {
            return db.DeliveryMen;
        }

        // GET: api/DeliveryMenApi/5
        [ResponseType(typeof(DeliveryMan))]
        public async Task<IHttpActionResult> GetDeliveryMan(int id)
        {
            DeliveryMan deliveryMan = await db.DeliveryMen.FindAsync(id);
            if (deliveryMan == null)
            {
                return NotFound();
            }

            return Ok(deliveryMan);
        }

        // PUT: api/DeliveryMenApi/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDeliveryMan(int id, DeliveryMan deliveryMan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != deliveryMan.DeliveryManID)
            {
                return BadRequest();
            }

            db.Entry(deliveryMan).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryManExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DeliveryMenApi
        [ResponseType(typeof(DeliveryMan))]
        public async Task<IHttpActionResult> PostDeliveryMan(DeliveryMan deliveryMan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DeliveryMen.Add(deliveryMan);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = deliveryMan.DeliveryManID }, deliveryMan);
        }

        // DELETE: api/DeliveryMenApi/5
        [ResponseType(typeof(DeliveryMan))]
        public async Task<IHttpActionResult> DeleteDeliveryMan(int id)
        {
            DeliveryMan deliveryMan = await db.DeliveryMen.FindAsync(id);
            if (deliveryMan == null)
            {
                return NotFound();
            }

            db.DeliveryMen.Remove(deliveryMan);
            await db.SaveChangesAsync();

            return Ok(deliveryMan);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeliveryManExists(int id)
        {
            return db.DeliveryMen.Count(e => e.DeliveryManID == id) > 0;
        }
    }
}