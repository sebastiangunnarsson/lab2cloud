using System.Security.Claims;
using System.Threading.Tasks;
using Lab2.Repositories;
using Microsoft.Owin.Security.OAuth;

namespace Lab2
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            context.Validated();
        }



        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin",new []{"*"});

            using (var _repo = new AuthRepository())
            {
                var user = await _repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant","The username or password is incorrect");
                    return;
                }

            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name,context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role,"user"));

            context.Validated(identity);
        }
    }
}