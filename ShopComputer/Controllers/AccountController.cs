using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopComputer.Data;
using ShopComputer.Models.DTOs.Admin;
using ShopComputer.Models.DTOs.Roles;

namespace ShopComputer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ShopUser> userManager;
        private readonly SignInManager<ShopUser> signInManager;

        public AccountController(UserManager<ShopUser> userManager,
            SignInManager<ShopUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto dTO)
        {
            if (!ModelState.IsValid)
                return View(dTO);
            ShopUser shopUser = new ShopUser
            {
                UserName = dTO.Username,
                Email = dTO.Email,
                DateOfBirth = dTO.DateOfBirth,
            };
            var result = await userManager.CreateAsync(shopUser, dTO.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(shopUser, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(dTO);
        }
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);
            ShopUser? shopUser = await userManager.FindByNameAsync(dto.Username);
            if (shopUser != null)
            {
                var result = await signInManager.PasswordSignInAsync(shopUser, dto.Password,
                    dto.RememberMe, false);
                if (await userManager.IsInRoleAsync(shopUser, "admin"))
                {
                    return RedirectToAction("UserList", "Roles");
                }
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Користувач/пароль не вірний");
                }
            }
            else
                ModelState.AddModelError(string.Empty, "Користувача не знайдено");
            return View(dto);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        } 

    }
}
