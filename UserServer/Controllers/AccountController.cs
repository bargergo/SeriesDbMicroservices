using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace UserServer.Controllers
{
    public class AccountController : Controller
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
    }
}
