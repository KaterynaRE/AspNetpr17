using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ShopComputer.Data;
using ShopComputer.Models.DTOs.Users;
using ShopComputer.Models.ViewModels.Users;
using System.Threading.Tasks;

namespace ShopComputer.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ShopUser> userManager;
        private readonly IMapper mapper;

        public UsersController(UserManager<ShopUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ShopUser> users = await userManager.Users.ToListAsync();
            IEnumerable<ShopUserDto> userDtos = mapper.Map<IEnumerable<ShopUserDto>>(users);
            return View(userDtos);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ShopUser? shopUser = await userManager.FindByIdAsync(id);
            if (shopUser == null)
            {
                return NotFound("Користувача не знайдено");
            }
            ShopUserDto shopUserDto = mapper.Map<ShopUserDto>(shopUser);
            return View(shopUserDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ShopUserDto shopUserDto)
        {
            if (!ModelState.IsValid)
            {
                return View(shopUserDto);
            }
            ShopUser? user = await userManager.FindByIdAsync(shopUserDto.Id);
            if (user != null)
            {
                user.DateOfBirth = shopUserDto.DateOfBirth;
                IdentityResult result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Не знайден");
            }
            return View(shopUserDto);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ShopUser? shopUser = await userManager.FindByIdAsync(id);
            if (shopUser == null)
            {
                return NotFound("Користувача не знайдено");
            }
            ShopUserDto shopUserDto = mapper.Map<ShopUserDto>(shopUser);
            return View(shopUserDto);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ShopUser? user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangePassword(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ShopUser? shopUser = await userManager.FindByIdAsync(id);
            if (shopUser == null)
            {
                return NotFound();
            }
            ChangePasswordVm pasw = new ChangePasswordVm()
            {
                Id = shopUser.Id,
                Email = shopUser.Email
            };
            return View(pasw);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVm cP)
        {
            if (!ModelState.IsValid)
            {
                return View(cP);
            }
            ShopUser? user = await userManager.FindByIdAsync(cP.Id);
            if (user == null)
            {
                return NotFound();
            }
            var result = await userManager.ChangePasswordAsync(user, cP.OldPassword, cP.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(cP);
        }

    }
}
