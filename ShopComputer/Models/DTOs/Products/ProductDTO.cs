using ShopComputer.Models.DTOs.Brands;
using ShopComputer.Models.DTOs.Categories;
using ShopComputerDomainLibrary;
using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models.DTOs.Products
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Display(Name = "Назва товару")]
        public string ProductName { get; set; } = default!;
        [Display(Name = "Опис товару")]
        public string Description { get; set; } = default!;
        [Display(Name = "Вартість")]
        public double Price { get; set; }

        [Display(Name = "Виробник")]
        public int BrandId { get; set; }

        [Display(Name = "Виробник")]
        public BrandDTO? Brand { get; set; } = default!;

        [Display(Name = "Категорія")]
        public CategoryDTO? Category { get; set; } = default!;

        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }

        [Display(Name = "Батьківська категорія")]
        public int? ParentCategoryId { get; set; }

        public ICollection<ProductImage>? ProductImages { get; set; } = default!;
    
    }
}
