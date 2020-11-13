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
using System.Threading.Tasks;
using UserServer.Entities;
using UserServer.Interfaces;
using UserServer.Models;

namespace UserServer.Controllers
{
    [Route("Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenSettings _tokenSettings;
        private readonly IUserService _userService;

        public AccountController(ITokenSettings _tokenSettings, IUserService userService)
        {
            this._tokenSettings = _tokenSettings;
            _userService = userService;
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
        public async Task<IActionResult> Authenticate(string returnUrl)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Challenge(OpenIdConnectDefaults.AuthenticationScheme);
            }

            
            var claims = new List<Claim>
            {
                new Claim("customclaim", "custom")
            };
            claims.AddRange(HttpContext.User.Claims);
            var email = claims.Find(c => c.Type == ClaimTypes.Email)?.Value;
            var firstname = claims.Find(c => c.Type == ClaimTypes.GivenName)?.Value;
            var lastname = claims.Find(c => c.Type == ClaimTypes.Surname)?.Value;
            try
            {
                var user = await _userService.FindUserByEmail(email);
            } catch
            {
                var user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    Firstname = firstname,
                    Lastname = lastname
                };
                await _userService.RegisterUser(user);
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

            return Redirect($"{returnUrl}?access_token={tokenJson}");//Ok(new { access_token = tokenJson, returnUrl });
        }

        [Authorize(AuthenticationSchemes = "MyJwtScheme")]
        [HttpGet("AuthenticateWithToken")]
        public IActionResult AuthenticateWithToken()
        {
            return Ok();
        }
    }
}
