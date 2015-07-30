using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BaseData.DataAccess;
using BaseData.Model;

namespace BaseData.Web.Controllers
{
    /// <summary>
    /// 账号管理API
    /// </summary>
    public class AccountsApiController : ApiController
    {
        private MyDataContext db = new MyDataContext();


        // GET: api/AccountsApi
        /// <summary>
        /// 获取所有注册账户
        /// </summary>
        /// <returns></returns>
        [ApiCompression]
        public IQueryable<Account> GetAccounts()
        {
            return db.Accounts;
        }

        // GET: api/AccountsApi/5
        /// <summary>
        /// 获取单一账户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ApiCompression]
        [ResponseType(typeof(Account))]
        public async Task<IHttpActionResult> GetAccount(string id)
        {
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }


        // PUT: api/AccountsApi/5
        /// <summary>
        /// 修改账号信息
        /// </summary>
        /// <param name="id">账号ID</param>
        /// <param name="account">账号对象</param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAccount(string id, Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != account.AccountID)
            {
                return BadRequest();
            }

            db.Entry(account).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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

        // POST: api/AccountsApi
        /// <summary>
        /// 账号注册
        /// </summary>
        /// <param name="account">accoubnt对象</param>
        /// <returns></returns>
        [ResponseType(typeof(Account))]
        [ApiCompression]
        public async Task<IHttpActionResult> PostAccount(Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Accounts.Add(account);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AccountExists(account.AccountID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = account.AccountID }, account);
        }

        // DELETE: api/AccountsApi/5
        /// <summary>
        /// 账号注销
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Account))]
        public async Task<IHttpActionResult> DeleteAccount(string id)
        {
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            db.Accounts.Remove(account);
            await db.SaveChangesAsync();

            return Ok(account);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AccountExists(string id)
        {
            return db.Accounts.Count(e => e.AccountID == id) > 0;
        }
    }
}