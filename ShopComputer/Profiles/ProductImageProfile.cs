using AutoMapper;
using ShopComputer.Models.DTOs.ProductImages;
using ShopComputerDomainLibrary;

namespace ShopComputer.Profiles
{
    public class ProductImageProfile: Profile
    {
        public ProductImageProfile()
        {
            CreateMap<ProductImage, ProductImageDTO>().ReverseMap();
        }
    }
}
