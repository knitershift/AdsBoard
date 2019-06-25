using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdsBoard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdsBoard.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new AppUser { Email = model.Email, UserName = model.Name };

                IdentityResult result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                    
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(String Id)
        {

            var user = await userManager.FindByIdAsync(Id);
            await userManager.DeleteAsync(user);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(String Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            ViewBag.UserID = user.Id;

            return View(new UserModel { Name = user.UserName, Email = user.Email } );
        }

        [HttpPost]
        public async Task<IActionResult> Edit(String Id, [FromForm] UserModel model)
        {
            var user = await userManager.FindByIdAsync(Id);

            user.Email = model.Email;
            user.UserName = model.Name;

            IdentityResult result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

            ViewBag.UserID = user.Id;

            return View(model);
        }
    }
}