using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models.DTOs.Brands
{
    public class BrandDTO
    {
        public int Id { get; set; }
        [Display(Name = "Назва бренду")]
        public string BrandName { get; set; } = default!;
        [Display(Name = "Країна реєстрації бренду")]
        public string Country { get; set; } = default!;
    }
}
