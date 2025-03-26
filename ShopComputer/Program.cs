using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopComputer.Data;
using ShopComputer.Profiles;

var builder = WebApplication.CreateBuilder(args);
string connStr = builder.Configuration.GetConnectionString("MSSQLShopDb") ??
    throw new InvalidOperationException("You should provide connection string!");

builder.Services.AddDbContext<ShopContext>(options =>
options.UseSqlServer(connStr));
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<ShopUser, IdentityRole>(
    options => {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
    })
    .AddEntityFrameworkStores<ShopContext>();

builder.Services.AddAutoMapper(typeof(ShopUserProfile), typeof(RoleProfile));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Account}/{action=Register}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
