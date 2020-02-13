using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Decode(string part)
        {
            var bytes = Convert.FromBase64String(part);
            return Ok(Encoding.UTF8.GetString(bytes));
        }
        [Authorize]
        public ActionResult Secret()
        {
            return View();
        }

        public ActionResult Authenticate()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,"some_id"),
                 new Claim("auth_sys","cookie")
            };
            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithms = SecurityAlgorithms.HmacSha256;
            var signinCredentils = new SigningCredentials(key,algorithms);
            var token = new JwtSecurityToken(Constants.Issuer,
                Constants.Audience,
                claims,notBefore:DateTime.Now,
                expires:DateTime.Now.AddHours(1),
                signinCredentils
                );
            var tokenJson =new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new {access_token=tokenJson });
        }
    }
}