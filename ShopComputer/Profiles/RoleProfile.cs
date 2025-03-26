using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ShopComputer.Models.DTOs.Roles;

namespace ShopComputer.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleDto>().ReverseMap();
        }
    }
}
