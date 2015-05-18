using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Lab2.Controllers
{
    [RoutePrefix("api/v1/users/{username}/friends")]
    public class FriendsController : BaseController
    {
        [Route("")]
        public async Task<IHttpActionResult> Get(string username)
        {
            return Ok(VacationUser.Friends.Select(f => new
            {
                f.Firstname,
                f.Lastname,
                f.Account.UserName
            }));
        }

        [Route("")]
        public async Task<IHttpActionResult> Post(string username, FriendDTO model)
        {
            if (User.Identity.Name != username)
                return Unauthorized();
            
            var friend = _ctx.Users.First(x => x.UserName == model.Username);
            
            if (friend == null)
                return BadRequest(string.Format("Non-existing user: {0}", model.Username));

            if (VacationUser.Friends.Contains(friend.VacationUser))
                return BadRequest(string.Format("You are already friends with {0}", model.Username));

            VacationUser.Friends.Add(friend.VacationUser);
            await _ctx.SaveChangesAsync();

            return Ok();
        }

        [Route("{friendname}")]
        public async Task<IHttpActionResult> Delete(string username, string friendname)
        {
            if (User.Identity.Name != username)
                return Unauthorized();

            var friend = _ctx.Users.First(x => x.UserName == friendname);

            if (friend == null)
                return BadRequest(string.Format("Non-existing user: {0}", friendname));

            if (!VacationUser.Friends.Contains(friend.VacationUser))
                return BadRequest(string.Format("You are not friends with {0}", friendname));

            VacationUser.Friends.Remove(friend.VacationUser);

            await _ctx.SaveChangesAsync();
            return Ok();

        }
    }
}
