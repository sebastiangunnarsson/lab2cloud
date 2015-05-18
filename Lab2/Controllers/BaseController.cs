using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Lab2.Models;

namespace Lab2.Controllers
{
    [Authorize]
    [Sharding]
    public class BaseController : ApiController
    {
        protected AppContext _ctx;

        public BaseController()
        {
            _ctx = new AppContext();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
                _ctx.Dispose();
            base.Dispose(disposing);
        }

        public User VacationUser
        {
            get
            {
                var identity = User.Identity as ClaimsIdentity;

                return _ctx.Users.First(x => x.UserName == identity.Name).VacationUser;
            }
        }
    }
}
