using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserServer.Models;

namespace UserServer.Controllers
{
    [Route("Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenSettings _tokenSettings;

        public AccountController(ITokenSettings _tokenSettings)
        {
            this._tokenSettings = _tokenSettings;
        }
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

        [HttpGet("Authenticate")]
        public IActionResult Authenticate()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Challenge(OpenIdConnectDefaults.AuthenticationScheme);
            }

            
            var claims = new List<Claim>
            {
                new Claim("customclaim", "custom")
            };
            foreach (var claim in HttpContext.User.Claims)
            {
                claims.Add(claim);
            }
            var secretBytes = Encoding.UTF8.GetBytes(_tokenSettings.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                _tokenSettings.Issuer,
                _tokenSettings.Audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signingCredentials
                );

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { access_token = tokenJson });
        }

        [Authorize]
        [HttpGet("Authorize")]
        public IActionResult Authorize()
        {
            return Ok();
        }
    }
}
