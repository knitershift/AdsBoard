using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdsBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AdsBoard.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManger)
        {
            this.userManager = userManager;
            this.signInManager = signInManger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;

            if (!ModelState.IsValid)
                return View(model);

            AppUser user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                Microsoft.AspNetCore.Identity.SignInResult result 
                    =  await signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                    return Redirect(ReturnUrl ?? "/");
            }

            return View(model);
        }

        
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
