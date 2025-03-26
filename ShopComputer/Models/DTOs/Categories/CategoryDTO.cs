using ShopComputerDomainLibrary;
using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models.DTOs.Categories
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        [Display(Name = "Назва категорії")]
        public string CategoryName { get; set; } = default!;
        [Display(Name = "Батьківська категорія")]
        public int? ParentCategoryId { get; set; }

        public CategoryDTO? ParentCategory { get; set; }

        public ICollection<CategoryDTO>? ChildCategories { get; set; } = default!;

        public ICollection<Product>? Products { get; set; } = default!;
    }
}
