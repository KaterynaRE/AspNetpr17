using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models.DTOs.Users
{
    public class ShopUserDto
    {
        public string Id { get; set; } = default!;

        [Display(Name = "Логін")]
        public string UserName { get; set; } = default!;

        public string Email { get; set; } = default!;

        [Display(Name = "Дата народження")]
        public DateOnly DateOfBirth { get; set; }

        [Display(Name = "Роль")]
        public List<string> Roles { get; set; } = new List<string>();
    }
}
