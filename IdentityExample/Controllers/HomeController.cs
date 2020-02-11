using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("index");
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string userName, string password)
        {
            var user = new IdentityUser
            {
                UserName = userName,
                Email = "",


            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var signInresult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInresult.Succeeded)
                {
                    return RedirectToAction("index");
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> SignOut()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}