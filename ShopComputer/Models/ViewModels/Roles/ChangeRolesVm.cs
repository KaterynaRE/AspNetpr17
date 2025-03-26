using Microsoft.AspNetCore.Identity;
using ShopComputer.Models.DTOs.Users;

namespace ShopComputer.Models.ViewModels.Roles
{
    public class ChangeRolesVm
    {
        public string Id { get; set; } = default!;

        public string? Email { get; set; } = default!;

        public IEnumerable<IdentityRole>? AllRoles { get; set; } = default!;

        public IList<string>? UserRoles { get; set; } = default!;

        public IEnumerable<string> Roles { get; set; } = default!;
    }
}
