using AspCoreAuthentication.CustomPolicyProvider;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspCoreAuthentication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public HomeController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public async Task<IActionResult> DoStuff([FromServices]IAuthorizationService service)
        {
            var builder = new AuthorizationPolicyBuilder("Schema");
            var customPolicy = builder.RequireClaim("Hello").Build();
            var result =await _authorizationService.AuthorizeAsync(HttpContext.User, customPolicy);
            if(result.Succeeded)
            {
                return View("Index");
            }
            return View("Index");
        }

        [Authorize(Roles ="Admin")]
        public IActionResult SecretRole()
        {
            return View("secret");
        }

        [SecurityLevel(5)]
        public IActionResult SecretLevel()
        {
            return View("secret");
        }

        [SecurityLevel(10)]
        public IActionResult SecretHighLevel()
        {
            return View("secret");
        }
        [AllowAnonymous]
        public IActionResult Authenticate()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Sachin"),
                new Claim(ClaimTypes.Email,"sachin@abc.com"),
                new Claim(ClaimTypes.DateOfBirth,"11/11/2000"),
                 new Claim(ClaimTypes.Role,"Admin"),
                      new Claim(DynamicPolycies.SecurityLevel,"10"),
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