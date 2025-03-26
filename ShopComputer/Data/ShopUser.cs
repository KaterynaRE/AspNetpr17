using Microsoft.AspNetCore.Identity;

namespace ShopComputer.Data
{
    public class ShopUser: IdentityUser
    {
        public DateOnly DateOfBirth { get; set; }
    }
}
