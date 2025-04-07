using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopComputer.Data;
using ShopComputer.Models.DTOs.Products;
using ShopComputer.Models.DTOs.Users;
using ShopComputer.Models.ViewModels.Home;
using ShopComputer.Models.ViewModels.Products;
using ShopComputerDomainLibrary;

namespace ShopComputer.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShopContext _context;
        private readonly IMapper mapper;

        public ProductsController(ShopContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: Products
        //public async Task<IActionResult> Index()
        //{
        //    var shopContext = _context.Products
        //        .Include(p => p.Brand)
        //        .Include(p => p.Category)
        //        .Include(i=>i.ProductImages);
        //    return View(await shopContext.ToListAsync());
        //}

        public async Task<IActionResult> Index(int page = 1)
        {
            int itemsPerPage = 3;
            IQueryable<Product> products = _context.Products
                .Include(b => b.Brand).Include(c => c.Category)
                .Include(i => i.ProductImages);
            int productsCount = products.Count();
            int totalPages = (int)Math.Ceiling((float)productsCount / itemsPerPage);
            products = products.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
            IndexProductVM indexVM = new IndexProductVM()
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Products = await products.ToListAsync()
            };
            return View(indexVM);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(i => i.ProductImages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "manager")]
        [Authorize(Policy = "managerPolicy")]
        public IActionResult Create()
        {
            CreateProductVM vM = new CreateProductVM()
            {
                Brands = new SelectList(_context.Brands, "Id", "BrandName"),
                Categories = new SelectList(_context.Categories, "Id", "CategoryName")
            };
            return View(vM);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "manager")]
        [Authorize(Policy = "managerPolicy")]
        public async Task<IActionResult> Create(CreateProductVM vM)
        {
            if (ModelState.IsValid)
            {
                Product product = mapper.Map<Product>(vM.ProductDTO);
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vM.Brands = new SelectList(_context.Brands, "Id", "BrandName", vM.ProductDTO.BrandId);
            vM.Categories = new SelectList(_context.Categories, "Id", "CategoryName", vM.ProductDTO.CategoryId);
            return View(vM);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "manager")]
        [Authorize(Policy = "managerPolicy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products
                .Include(b=>b.Brand)
                .Include(c=>c.Category)
                .ThenInclude(c=>c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ProductDTO productDTO = mapper.Map<ProductDTO>(product);
            ViewBag.BrandId = new SelectList(_context.Brands, "Id", "BrandName", product.BrandId);
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "CategoryName", product.CategoryId);
            ViewBag.ParentCategoryId = new SelectList(_context.Categories.Where(c => c.ParentCategoryId == null), "Id", "CategoryName", product.Category?.ParentCategoryId);
            return View(productDTO);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "manager")]
        [Authorize(Policy = "managerPolicy")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,Description,Price,BrandId,CategoryId, ParentCategoryId")] ProductDTO editProduct)
        {
            if (id != editProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   var prod = await _context.Products.FindAsync(id);
                    if (prod == null)
                    {
                        return NotFound();
                    }
                    prod.ProductName = editProduct.ProductName;
                    prod.Description = editProduct.Description;
                    prod.Price = editProduct.Price;
                    prod.BrandId = editProduct.BrandId;
                    prod.CategoryId = editProduct.CategoryId;

                    _context.Update(prod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(editProduct.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.BrandId = new SelectList(_context.Brands, "Id", "BrandName", editProduct.BrandId);
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "CategoryName", editProduct.CategoryId);
            ViewBag.ParentCategoryId = new SelectList(_context.Categories.Where(c => c.ParentCategoryId == null), "Id", "CategoryName", editProduct?.ParentCategoryId);
            

            return View(editProduct);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "manager")]
        [Authorize(Policy = "managerPolicy")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "manager")]
        [Authorize(Policy = "managerPolicy")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

      


    }
}
