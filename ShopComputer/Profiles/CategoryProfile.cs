using AutoMapper;
using ShopComputer.Models.DTOs.Categories;
using ShopComputerDomainLibrary;

namespace ShopComputer.Profiles
{
    public class CategoryProfile: Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
