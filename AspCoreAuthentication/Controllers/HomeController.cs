using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreAuthentication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }
        public IActionResult Authenticate()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Sachin"),
                new Claim(ClaimTypes.Email,"sachin@abc.com"),
                new Claim("My.Love","Sachin"),
            };

            var licenceClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Sachin K Patel"),
                new Claim("Licence","Windows licence")
            };

            var identity = new ClaimsIdentity(claims,"myIdentity");
            var principle = new ClaimsPrincipal(new[] {identity });

            HttpContext.SignInAsync(principle);
            return RedirectToAction("Index");
        }
    }
}