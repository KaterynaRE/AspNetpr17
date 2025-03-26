using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models.ViewModels.Users
{
    public class ChangePasswordVm
    {
        public string Id { get; set; } = default!;

        public string? Email { get; set; } = default!;

        [Display(Name = "Введіть поточний пароль")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = default!;

        [Display(Name = "Введіть новий пароль")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = default!;

        [Display(Name = "Підтвердіть пароль")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; } = default!;
    }
}
