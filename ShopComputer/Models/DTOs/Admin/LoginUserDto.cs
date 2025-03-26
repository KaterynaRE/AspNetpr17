using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models.DTOs.Admin
{
    public class LoginUserDto
    {
        //public int Id { get; set; }
        [Required]
        [Display(Name = "Логін")]
        public string Username { get; set; } = default!;

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
        [Display(Name = "Залишатися в системі")]

        public bool RememberMe { get; set; }
    }
}
