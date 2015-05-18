using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Lab2.Helpers;
using Lab2.Models;

namespace Lab2.Controllers.V1
{
    [RoutePrefix("api/v1/vacations")]
    [Lab2.Helpers.Caching]
    public class VacationsController : BaseController
    {
        #region https://server/api/v1/vacations

        //GET - Retrieves a list of vacations
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {

            var cache = CacheHelper.Get<object>(HttpContext.Current.Request.RawUrl);

            if (cache == null)
            {
                var retval = _ctx.Vacations.Select(x => new
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
                    }).ToList()
                }).ToList();

                CacheHelper.Save(HttpContext.Current.Request.RawUrl,retval);
                return Ok(retval);
            }
            else
            {
                return Ok(cache);
            }
        }

        //POST - Creates a new vacation for current user
        [Route("")]
        public async Task<IHttpActionResult> Post(VacationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            Vacation vac = new Vacation()
            {
                Description = model.Description,
                Title = model.Title,
                Place = model.Place,
                StartDate = model.StartDate ?? DateTime.MinValue,
                EndDate = model.EndDate ?? DateTime.MinValue,
                User = VacationUser
            };

            _ctx.Vacations.Add(vac);
            await _ctx.SaveChangesAsync();
            return Ok();
        }

        #endregion

        #region https://server/api/v1/vacations/12

        //GET - Retrieves a specific vacation
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var vacation = await _ctx.Vacations.FindAsync(id);
            if (vacation == null)
                return BadRequest("Invalid id");


            return Ok(new
            {
                vacation.Id,
                vacation.Title,
                vacation.Description,
                vacation.Place,
                vacation.StartDate,
                vacation.EndDate,
                UserId = vacation.User.Id,
                Memories = vacation.Memories.Select(m => new
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
            });
        }

        //PUT - Updates vacation #12
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, VacationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vac = await _ctx.Vacations.FindAsync(id);
            if (vac == null)
                return BadRequest("Invalid id");


            //Update all fields from DTO.
            vac.Description = model.Description;
            vac.Title = model.Title;
            vac.Place = model.Place;
            vac.StartDate = model.StartDate.Value;
            vac.EndDate = model.EndDate.Value;

            await _ctx.SaveChangesAsync();
            return Ok();
        }

        //PATCH - Partially updates ticket #12
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Patch(int id, VacationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vac = await _ctx.Vacations.FindAsync(id);
            if (vac == null)
                return BadRequest("Invalid id");

            Type modelType = model.GetType();
            foreach (var prop in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                //Kolla vilka properties som ligger med i DTO:n, ändra endast dem.
            {
                object val = prop.GetValue(model);
                if (val is string && !string.IsNullOrEmpty(val.ToString()))
                    vac.GetType().GetProperty(prop.Name).SetValue(vac, val);
                else if (val is DateTime? && (val as DateTime?).HasValue)
                    vac.GetType().GetProperty(prop.Name).SetValue(vac, val);
            }
            await _ctx.SaveChangesAsync();
            return Ok();
        }

        //DELETE - Delete vacation #12
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var vac = await _ctx.Vacations.FindAsync(id);
            if (vac == null)
                return BadRequest("Invalid id");

            _ctx.Vacations.Remove(vac);
            await _ctx.SaveChangesAsync();

            return Ok();
        }

        #endregion

        #region https://server/api/v1/users/psmith/vacations

        //GET - Retrieves a list of user psmiths vacations
        [Route("~/api/v1/users/{username}/vacations")]
        public async Task<IHttpActionResult> GetVacationsForUser(string username)
        {
            return Ok(_ctx.Vacations.Where(v => v.User.Account.UserName == username).Select(x => new
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

        #endregion
    }
}