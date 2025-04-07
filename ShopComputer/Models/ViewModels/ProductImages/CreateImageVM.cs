using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models.ViewModels.ProductImages
{
    public class CreateImageVM
    {
        [Display(Name = "Бренди")]
        public SelectList? Brands { get; set; }
        [Display(Name = "Оберіть Бренд")]
        public int? SelectedBrandId { get; set; }
        [Display(Name = "Категорії")]
        public SelectList? Categories { get; set; }

        [Display(Name = "Оберіть Категорію")]
        public int? SelectedCategoryId { get; set; }
        [Display(Name = "Товари")]
        public SelectList? Products { get; set; }
        [Display(Name = "Товар")]
        public int SelectedProductId { get; set; }
        [Display(Name = "Фото")]
        public IFormFile[] Photos { get; set; } = default!;
    }
}
