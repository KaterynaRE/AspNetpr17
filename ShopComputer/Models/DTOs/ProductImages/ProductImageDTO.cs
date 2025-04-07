using ShopComputer.Models.DTOs.Products;
using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models.DTOs.ProductImages
{
    public class ProductImageDTO
    {
        public int Id { get; set; }
        [Display(Name = "Фото")]
        public byte[]? ImageData { get; set; } = default!;
        [Display(Name = "Товар")]
        public int ProductId { get; set; }
        [Display(Name = "Товар")]
        public ProductDTO? Product { get; set; } = default!;

    }
}
