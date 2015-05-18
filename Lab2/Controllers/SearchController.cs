using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Lab2.Models;

namespace Lab2.Controllers.V1
{
    [RoutePrefix("api/v1/search")]
    public class SearchController : BaseController
    {
        [Route("")]
        public async Task<IHttpActionResult> Get(string type, string q)
        {
            IEnumerable<Vacation> query;

            switch (type)
            {
                case "user":
                    query = new List<Vacation>();
                        await _ctx.AppUsers.Where(x => x.Account.UserName.ToLower() == q.ToLower())
                            .Select(x => x.TaggedInMemories.Select(y => y.Vacation)).ForEachAsync(o =>
                            {
                                var list = query as List<Vacation>;


                                foreach (var el in o)
                                {
                                    if(!list.Contains(el))
                                        list.Add(el);
                                }
                            });
                    break;
                case "tag":
                    query = _ctx.SearchTags.First(x => x.Tag.ToLower() == q.ToLower()).Memories.Select(x => x.Vacation);
                    break;
                case "place":
                    query = _ctx.Vacations.Where(x => x.Place.ToLower() == q.ToLower());
                    break;

                case "title":
                    query = _ctx.Vacations.Where(x => x.Title.ToLower().Contains(q.ToLower()));
                    break;

                default:
                    return BadRequest("Type is missing");
            }


            return Ok(query.ToList().Select(x => new
            {
                x.Id,
                x.Title,
                x.Description,
                x.Place,
                x.StartDate,
                x.EndDate,
                UserId = x.User.Id,
                Memories = x.Memories.Select(m => new
                {
                    m.Id,
                    m.Description,
                    m.Date,
                    TaggedUsers = m.TaggedUsers.Select(u => new
                    {
                        u.Account.UserName,
                        u.Firstname,
                        u.Lastname
                    }),
                    SearchTags = m.SearchTags.Select(t => new
                    {
                        t.Tag
                    }),
                    m.Position
                })
            }));
        }
    }
}
