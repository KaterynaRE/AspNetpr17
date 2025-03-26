using AutoMapper;
using ShopComputer.Data;
using ShopComputer.Models.DTOs.Users;

namespace ShopComputer.Profiles
{
    public class ShopUserProfile: Profile
    {
        public ShopUserProfile()
        {
            CreateMap<ShopUser, ShopUserDto>().ReverseMap();
        }
    }
}
