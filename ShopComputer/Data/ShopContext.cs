using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopComputerDomainLibrary;
using ShopComputer.Models.DTOs.Admin;
using ShopComputer.Models.DTOs.Users;
using ShopComputer.Models.ViewModels.Users;
using ShopComputer.Models.DTOs.Roles;
using ShopComputer.Models.DTOs.Brands;

namespace ShopComputer.Data
{
    public class ShopContext: IdentityDbContext<ShopUser>
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        { }
        public DbSet<Brand> Brands { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> Images { get; set; }
       // public DbSet<ShopComputer.Models.DTOs.Brands.BrandDTO> BrandDTO { get; set; } = default!;
      // public DbSet<ShopComputer.Models.DTOs.Roles.RoleDto> RoleDto { get; set; } = default!;
       // public DbSet<ShopComputer.Models.DTOs.Users.ShopUserDto> ShopUserDto { get; set; } = default!;
        //public DbSet<ShopComputer.Models.ViewModels.Users.ChangePassword> ChangePassword { get; set; } = default!;
       // public DbSet<ShopComputer.Models.DTOs.Users.ShopUserDto> ShopUserDto { get; set; } = default!;
        //public DbSet<ShopComputer.Models.DTOs.Admin.LoginUserDto> LoginUserDto { get; set; } = default!;
    }
}
