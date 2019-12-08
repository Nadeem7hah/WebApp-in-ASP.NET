using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityCoreDemo.Models;
using IdentityCoreDemo.ViewModels;

namespace IdentityCoreDemo.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<AppUser> _signInManager;
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleMgr)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleMgr;
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, loginModel.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Faild to Login");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Course");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                AppUser student = new AppUser
                {
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    UserName = registerModel.UserName,
                    PhoneNumber = registerModel.PhoneNumber,
                    Email = registerModel.Email
                };

                var result = await _userManager.CreateAsync(student, registerModel.Password);
                if (result.Succeeded)
                {
                    var addedUser = await _userManager.FindByNameAsync(registerModel.UserName);
                    await _userManager.AddToRoleAsync(addedUser, "User");
                    return RedirectToAction("Login", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }


        [Route("CreateRoles")]
        public async Task<IActionResult> CreateRoles()
        {
            try
            {
                string[] roleNames = { "Administrator", "Manager", "User" };
                foreach (var roleName in roleNames)
                {
                    bool roleExists = await _roleManager.RoleExistsAsync(roleName);
                    if (!roleExists)
                    {
                        IdentityRole role = new IdentityRole();
                        role.Name = roleName;
                        await _roleManager.CreateAsync(role);
                    }
                }
                return Content("Created");
            }
            catch (Exception ex)
            {

                return Content(ex.Message);
            }

        }
    }
}