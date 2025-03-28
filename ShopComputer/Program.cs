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


builder.Services.AddAuthentication().AddGoogle(options => {
    IConfigurationSection googleSection = builder.Configuration.GetSection("Authentication:Google");
    string clientId = googleSection.GetValue<string>("ClientId") ??
    throw new InvalidOperationException("Please provide ClientId!");
    string clientSecret = googleSection.GetValue<string>("ClientSecret") ??
    throw new InvalidOperationException("Please provide Client Secret!");
    options.ClientId = clientId;
    options.ClientSecret = clientSecret;
});

builder.Services.AddAuthorization(configure =>
{
    configure.AddPolicy("managerPolicy", policyBuilder =>
    {
        //policyBuilder.RequireRole("manager");
        policyBuilder.RequireAssertion(context =>
     context.User.IsInRole("manager") && !context.User.IsInRole("admin") && !context.User.IsInRole("user"));

        policyBuilder.RequireAuthenticatedUser();
    });
});

builder.Services.AddAutoMapper(typeof(ShopUserProfile), typeof(RoleProfile), 
    typeof(BrandProfile), typeof(CategoryProfile));

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
