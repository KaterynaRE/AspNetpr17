using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopComputer.Data;
using ShopComputer.Models.ViewModels.Claims;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopComputer.Controllers
{
    [Authorize(Roles = "admin")]
    [Authorize(Policy = "adminAge")]
    public class ClaimsController : Controller
    {
        private readonly UserManager<ShopUser> userManager;
        private readonly SignInManager<ShopUser> signInManager;

        public ClaimsController(UserManager<ShopUser> userManager,
            SignInManager<ShopUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            if (User != null && User.Identity != null 
                && User.Identity.IsAuthenticated)
            {
                var claims = User.Claims;
                ShopUser? shopUser = await userManager.FindByNameAsync(User.Identity.Name!);
                if (shopUser == null)
                {
                    return NotFound();
                }
                IndexClaimsVm vM = new IndexClaimsVm()
                {
                    Claims = claims,
                    UserName = shopUser.UserName!,
                    Email = shopUser.Email!
                };
                return View(vM);
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string claimType, string claimValue)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Claim claim = new Claim(claimType, claimValue, ClaimValueTypes.String);
            ShopUser? shopUser = await userManager.GetUserAsync(User);
            if (shopUser == null)
            {
                return NotFound();
            }
           var result = await userManager.AddClaimAsync(shopUser, claim);
            if (result.Succeeded)
            {
                await signInManager.SignOutAsync();
                await signInManager.SignInAsync(shopUser, false);
                return RedirectToAction("Index");
            }
            Errors(ModelState, result);
            return View();
        }


        public void Errors(ModelStateDictionary modelState, IdentityResult  identityResult)
        {
            foreach (var error in identityResult.Errors )
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string claimValues)
        {
            if (User != null && User.Identity != null)
            {
                string[] claimsData = claimValues.Split(';');
                string claimType = claimsData[0];
                string claimValue = claimsData[1];
                string claimIssuer = claimsData[2];
                Claim? claim = User.Claims.FirstOrDefault(t=>
                t.Type == claimType && 
                t.Value == claimValue &&
                t.Issuer == claimIssuer);
                if (claim != null)
                {
                    ShopUser? shopUser = await userManager.GetUserAsync(User);
                    if (shopUser == null)
                    {
                        return NotFound();
                    }
                    await userManager.RemoveClaimAsync(shopUser, claim);
                    await signInManager.SignOutAsync();
                    await signInManager.SignInAsync(shopUser, false);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Login", "Account");
        }

    }
}
