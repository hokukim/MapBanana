using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MapBanana.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task Login(string returnUrl = "/")
        {
            AuthenticationProperties authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                // Indicate here where Auth0 should redirect the user after a login.
                // Note that the resulting absolute Uri must be added to the
                // **Allowed Callback URLs** settings for the app.
                .WithRedirectUri(returnUrl)
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }
    }
}
