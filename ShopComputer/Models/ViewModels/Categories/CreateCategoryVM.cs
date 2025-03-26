using Microsoft.AspNetCore.Mvc.Rendering;
using ShopComputer.Models.DTOs.Categories;

namespace ShopComputer.Models.ViewModels.Categories
{
    public class CreateCategoryVM
    {
        public CategoryDTO CategoryDTO { get; set; } = default!;

        public SelectList? ParentCategories { get; set; }
        //public SelectList? ChildCategories { get; set; }
    }
}
