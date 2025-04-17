using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopComputer.Data;
using ShopComputer.Models.ViewModels.Home;
using ShopComputerDomainLibrary;
using System.Threading.Tasks;

namespace ShopComputer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopContext context;

        public HomeController(ShopContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int itemsPerPage = 3;
            IQueryable<Product> products = context.Products
                .Include(b=>b.Brand)
                .Include(c=>c.Category)
                .Include(i => i.ProductImages);
            int productsCount = products.Count();
            int totalPages = (int)Math.Ceiling((float)productsCount/itemsPerPage);
            products = products.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
            HomeIndexVM homeIndexVM = new HomeIndexVM()
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Products = await products.ToListAsync()
            };
            return View(homeIndexVM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await context.Products
                 .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
