using Microsoft.AspNetCore.Mvc;
using ShopComputer.Data;
using ShopComputerDomainLibrary;
using ShopComputer.Extensions;
using ShopComputer.Models.ViewModels.Cart;
using Microsoft.EntityFrameworkCore;


namespace ShopComputer.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopContext _context;
        string key = "cart";
        public CartController(ShopContext context)
        {
            this._context = context;
        }
        public IActionResult Index(Cart cart, string? returnUrl)
        {
            //Cart cart = GetCart();
            CartIndexVM vM = new CartIndexVM()
            {
                Cart = cart,
                ReturnUrl = returnUrl
            };
            return View(vM);
        }

        public async Task<IActionResult> AddToCart(int? id, Cart cart, string? returnUrl)
        {
            if (id == null)
                return NotFound();
            Product? product = await _context.Products
                .Include(t => t.Brand)
                .Include(t => t.ProductImages)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (product == null)
                return NotFound();
            //Cart cart = GetCart();
            cart.Add(new CartItem { Product = product, CountProduct = 1 });
            //HttpContext.Session.Set(key, cart.CartItems);
            return RedirectToAction("Index", new { returnUrl });
        }

        public async Task<IActionResult> RemoveOneCart(int? id, Cart cart, string? returnUrl)
        {
            if (id == null)
                return NotFound();
            Product? product = await _context.Products
                .Include(b => b.Brand)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(i=>i.Id == id);
            if (product == null)
                return NotFound();
            cart.DecreaseOne(new CartItem { Product = product, CountProduct = 1 });
            return RedirectToAction("Index", new { returnUrl });
        }


        [HttpPost]
        public IActionResult RemoveFromCart(Cart cart, int? id, string? returnUrl)
        {
            if (id == null)
                return NotFound();
            cart.Remove(id.Value);
            //HttpContext.Session.Set(key, cart.CartItems);
            return RedirectToAction("Index", new { returnUrl });
        }


        public IActionResult SetUser()
        {
            HttpContext.Session.SetString("CurrentUser", "Reva");
            return View();
        }

        public IActionResult Show()
        {
            string? userName = HttpContext.Session.GetString("CurrentUser");
            return View(model: userName);
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Clear();
            return View();
        }

        //public Cart GetCart()
        //{
        //    List<CartItem>? cartItems =
        //        HttpContext.Session.Get<List<CartItem>>(key);

        //    if (cartItems == null)
        //    {
        //        cartItems = new List<CartItem>();
        //        HttpContext.Session.Set(key, cartItems);
        //    }
        //    Cart cart = new Cart(cartItems);
        //    return cart;
        //}

    }
}
