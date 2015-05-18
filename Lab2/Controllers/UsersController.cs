using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Lab2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab2.Controllers.V1
{
    [RoutePrefix("api/v1/users/{username}")]
    public class UsersController : BaseController
    {
        [Route("")]
        public async Task<IHttpActionResult> Get(string username)
        {
            var user = _ctx.Users.FirstOrDefault(x => x.UserName == username);

            if (user == null)
                return BadRequest("Unknown user");

            return Ok(new
            {
                user.UserName,
                user.Email,
                user.VacationUser.Firstname,
                user.VacationUser.Lastname
            });
        }

        [Route("")]
        public async Task<IHttpActionResult> Put(string username, UserDTO model)
        {
            if (User.Identity.Name != username)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _ctx.Users.FirstOrDefault(x => x.UserName == username);

            if (user == null)
                return BadRequest("Unknown user");

            user.Email = model.Email;
            user.VacationUser.Firstname = model.Firstname;
            user.VacationUser.Lastname = model.Lastname;

            await _ctx.SaveChangesAsync();
            return Ok();
        }

        [Route("")]
        public async Task<IHttpActionResult> Patch(string username, UserDTO model)
        {
            if (User.Identity.Name != username)
            {
                return Unauthorized();
            }

            var user = _ctx.Users.FirstOrDefault(x => x.UserName == username);

            if (user == null)
                return BadRequest("Unknown user");


            if (!string.IsNullOrEmpty(model.Email))
                user.Email = model.Email;

            if (!string.IsNullOrEmpty(model.Firstname))
                user.VacationUser.Firstname = model.Firstname;

            if (!string.IsNullOrEmpty(model.Lastname))
                user.VacationUser.Lastname = model.Lastname;

            await _ctx.SaveChangesAsync();

            return Ok();
        }
    }
}