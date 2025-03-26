using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models.DTOs.Roles
{
    public class RoleDto
    {
        public string Id { get; set; } = default!;
        [Display(Name = "Назва ролі")]
        public string Name { get; set; } = default!;
    }
}
