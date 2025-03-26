using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models.DTOs.Admin
{
    public class RegisterUserDto
    {
        [Required]
        [Display(Name = "Логін")]
        public string Username { get; set; } = default!;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        [Required]
        [Display(Name = "Дата народження")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [Required]
        [Display(Name = "Підтвердження пароля")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; } = default!;
    }
}
