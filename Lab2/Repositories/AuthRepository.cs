using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lab2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab2.Repositories
{
    public class AuthRepository : IDisposable
    {
        private AppContext _ctx;
        private UserManager<CustomIdentityUser> _userManager;

        public AuthRepository()
        {
            _ctx = new AppContext();
            _userManager = new UserManager<CustomIdentityUser>(new UserStore<CustomIdentityUser>(_ctx));
        }
        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }

        public async Task<IdentityResult> RegisterUser(AccountDTO userModel)
        {
            CustomIdentityUser user = new CustomIdentityUser()
            {
                UserName = userModel.Username,
                Email = userModel.Email
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result == IdentityResult.Success)
            {
                var vacUser = new User {Account = user, Firstname = userModel.Firstname, Lastname = userModel.Lastname};
                _ctx.AppUsers.Add(vacUser);
                user.VacationUser = vacUser;
                _ctx.SaveChanges();
            }
            return result;

        }

        public async Task<CustomIdentityUser> FindUser(string username, string password)
        {
            var user = await _userManager.FindAsync(username, password);
            return user;
        }
    }
}
