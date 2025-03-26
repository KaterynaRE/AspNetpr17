using System.Security.Claims;

namespace ShopComputer.Models.ViewModels.Claims
{
    public class IndexClaimsVm
    {
        public IEnumerable<Claim>? Claims { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
