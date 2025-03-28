using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopComputer.Data;
using ShopComputer.Models.DTOs.Roles;
using ShopComputer.Models.DTOs.Users;
using ShopComputer.Models.ViewModels.Roles;

namespace ShopComputer.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ShopUser> userManager;
        private readonly IMapper mapper;

        public RolesController(RoleManager<IdentityRole> roleManager,
            UserManager<ShopUser> userManager, IMapper mapper)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await roleManager.Roles.ToListAsync();
            IEnumerable<RoleDto> roleDtos = mapper.Map<IEnumerable<RoleDto>>(roles);
            return View(roleDtos);
        }

        
        public IActionResult Create() => View();

        [HttpPost]
       
        public async Task<IActionResult> Create(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError(string.Empty, "Не може бути порожня");
                return View(model: roleName);
            }
            IdentityRole role = new IdentityRole { Name = roleName };
            await roleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }

        //public async Task<ActionResult> UserList()
        //{
        //    IEnumerable<ShopUser> users = await userManager.Users.ToListAsync();
        //    IEnumerable<ShopUserDto> userDtos = mapper.Map<IEnumerable<ShopUserDto>>(users);
        //    return View(userDtos);
        //}

       
        public async Task<ActionResult> UserList()
        {
            var users = await userManager.Users.ToListAsync();
            var userDtos = mapper.Map<IEnumerable<ShopUserDto>>(users);

            foreach (var userDto in userDtos)
            {
                var user = users.FirstOrDefault(u => u.Id == userDto.Id);
                if (user != null)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    userDto.Roles = roles.ToList();
                }
            }

            return View(userDtos);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeRoles(string? id)
        {
            if (id == null)
                return NotFound();
            ShopUser? shopUser = await userManager.FindByIdAsync(id);
            if (shopUser == null)
                return NotFound();
            var allRoles = await roleManager.Roles.ToListAsync();
            var userRoles = await userManager.GetRolesAsync(shopUser);
            ChangeRolesVm vM = new ChangeRolesVm()
            {
                Id = shopUser.Id,
                Email = shopUser.Email,
                AllRoles = allRoles,
                UserRoles = userRoles
            };
            return View(vM);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRoles(ChangeRolesVm vM)
        {
            ShopUser? shopUser = await userManager.FindByIdAsync(vM.Id);
            if (shopUser == null) return NotFound();
            var allRoles = await roleManager.Roles.ToListAsync();

            var userRoles = await userManager.GetRolesAsync(shopUser);
            if (ModelState.IsValid)
            {
                var addedRoles = vM.Roles.Except(userRoles);
                var deletedRoles = userRoles.Except(vM.Roles);
                await userManager.AddToRolesAsync(shopUser, addedRoles);
                await userManager.RemoveFromRolesAsync(shopUser, deletedRoles);
                return RedirectToAction("UserList");
            }
            vM.AllRoles = allRoles;
            vM.UserRoles = userRoles;
            vM.Email = shopUser.Email;
            return View(vM);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IdentityRole? role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound("Такої ролі не існує");
            }
            RoleDto newRole = mapper.Map<RoleDto>(role);
            return View(newRole);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return View(roleDto);
            }
            IdentityRole? role = await roleManager.FindByIdAsync(roleDto.Id);

            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "Роль не знайдена");
                return View(roleDto);
            }
            if (role != null)
            {
                role.Name = roleDto.Name;
                IdentityResult result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Не знайден");
            }
            return View(roleDto);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IdentityRole? role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound("Роль не знайдено");
            }
            RoleDto roleDto = mapper.Map<RoleDto>(role);
            return View(roleDto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IdentityRole? role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            await roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }


    }
}
