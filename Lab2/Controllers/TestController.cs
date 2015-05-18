using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Lab2.Models;
using Microsoft.AspNet.Identity;

namespace Lab2.Controllers.V1
{
    [RoutePrefix("api/v1/Test")]
    public class TestController : ApiController
    {
        [Authorize]
        public string Get()
        {
            var identity = User.Identity as ClaimsIdentity;

            var role = identity.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            Models.User appUser;

            using (var db = new AppContext())
            {
                appUser = db.Users.First(u => u.UserName == identity.Name).VacationUser;
            }

            return appUser.Firstname + " " + appUser.Lastname + " " + role;
        }
    }
}
