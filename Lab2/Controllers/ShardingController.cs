using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Lab2.Controllers
{
    public class ShardingController : BaseController
    {
        [Route("api/v1/sharding")]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await _ctx.Shards.SumAsync(x => x.Counter));
        }
    }
}
