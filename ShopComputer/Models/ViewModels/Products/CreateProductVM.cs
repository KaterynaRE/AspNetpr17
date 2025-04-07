using Microsoft.AspNetCore.Mvc.Rendering;
using ShopComputer.Models.DTOs.Products;

namespace ShopComputer.Models.ViewModels.Products
{
    public class CreateProductVM
    {
        public ProductDTO ProductDTO { get; set; } = default!;

        public SelectList? Brands { get; set; }

        public SelectList? Categories { get; set; }
    }
}
