using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Lab2.Models;

namespace Lab2.Controllers
{
    public class ShardingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            using (var _ctx = new AppContext())
            {
                Random rnd = new Random();

                var shard = _ctx.Shards.ToList()[rnd.Next(0, _ctx.Shards.Count())];
                shard.Counter++;

                _ctx.SaveChanges();

            }

        }
    }
}