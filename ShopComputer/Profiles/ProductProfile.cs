using AutoMapper;
using ShopComputer.Models.DTOs.Products;
using ShopComputerDomainLibrary;

namespace ShopComputer.Profiles
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
