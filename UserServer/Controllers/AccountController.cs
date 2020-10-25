using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserServer.Controllers
{
    [Route("Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Challenge(OpenIdConnectDefaults.AuthenticationScheme);
            }
            return Ok();
            //return RedirectToAction("Index", "Home");
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            var callbackUrl = Url.Action("Index", "Home");
            return new SignOutResult(new[]
            {
                CookieAuthenticationDefaults.AuthenticationScheme
            },
            new AuthenticationProperties { RedirectUri = callbackUrl }
            );
        }

        [Authorize]
        [HttpGet("Authorize")]
        public IActionResult Authorize()
        {
            return Ok();
        }
    }
}
