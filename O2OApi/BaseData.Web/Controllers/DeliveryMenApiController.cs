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
    /// <summary>
    /// 配送员API
    /// </summary>
    public class DeliveryMenApiController : ApiController
    {
        private MyDataContext db = new MyDataContext();
        
        /// <summary>
        /// 获取所有配送员列表
        /// </summary>
        /// <returns></returns>
          [ApiCompression]
        public IQueryable<DeliveryMan> GetDeliveryMen()
        {
            return db.DeliveryMen;
        }
        /// <summary>
        /// 根据ID获取指定配送员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(DeliveryMan))]
        [ApiCompression]
        public async Task<IHttpActionResult> GetDeliveryMan(int id)
        {
            DeliveryMan deliveryMan = await db.DeliveryMen.FindAsync(id);
            if (deliveryMan == null)
            {
                return NotFound();
            }

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