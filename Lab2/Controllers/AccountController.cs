using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Lab2.Models;
using Lab2.Repositories;
using Microsoft.AspNet.Identity;

namespace Lab2.Controllers.V1
{
    [RoutePrefix("api/v1/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository _repo;

        public AccountController()
        {
            _repo = new AuthRepository();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _repo.Dispose();
            base.Dispose(disposing);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(AccountDTO userModel)
        {
            if (userModel == null)
            {
                return BadRequest("No parameters supplied");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.RegisterUser(userModel);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }
            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }
                return BadRequest(ModelState);
            }
            return null;
        }
    }
}