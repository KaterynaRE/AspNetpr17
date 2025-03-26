using AutoMapper;
using ShopComputer.Models.DTOs.Brands;
using ShopComputerDomainLibrary;

namespace ShopComputer.Profiles
{
    public class BrandProfile: Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, BrandDTO>().
                ReverseMap();
        }
    }
}
