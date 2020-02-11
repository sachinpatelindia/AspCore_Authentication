using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;

namespace IdentityExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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
                //generat email token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action(nameof(VeryfyEmail),"Home",new {userId=user.Id,code },Request.Scheme,Request.Host.ToString());
                await _emailService.SendAsync("sachin@skpatel.net", "email verification", link);// $"<a href=\"{link}\">Veryfy emil</a>");
                var signInresult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInresult.Succeeded)
                {
                    return RedirectToAction("index");
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VeryfyEmail(string userId, string code)
        {
            var user =await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest();
            var result= await _userManager.ConfirmEmailAsync(user,code);
            if (result.Succeeded) return View();
            return BadRequest();
        }

        public IActionResult EmailVerification() => View();
        public async Task<IActionResult> SignOut()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}