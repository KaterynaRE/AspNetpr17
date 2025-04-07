using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopComputer.Data;
using ShopComputer.Models.DTOs.ProductImages;
using ShopComputer.Models.DTOs.Products;
using ShopComputer.Models.ViewModels.ProductImages;
using ShopComputerDomainLibrary;

namespace ShopComputer.Controllers
{
    [Authorize(Roles = "manager")]
    [Authorize(Policy = "managerPolicy")]
    public class ProductImagesController : Controller
    {
        private readonly ShopContext _context;
        private readonly IMapper _mapper;

        public ProductImagesController(ShopContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        // GET: ProductImages
        public async Task<IActionResult> Index()
        {
            var shopContext = _context.Images.Include(p => p.Product);
            return View(await shopContext.ToListAsync());
        }

        // GET: ProductImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImage = await _context.Images
                  .Include(p => p.Product)
                .ThenInclude(p => p.ProductImages)
              .Include(p => p.Product)
                  .ThenInclude(p => p.Brand)
                  .FirstOrDefaultAsync(m => m.Id == id);
            if (productImage == null || productImage.Product == null)
            {
                return NotFound();
            }


            return View(productImage);
        }

        // GET: ProductImages/Create
        public async Task<IActionResult> Create(int? selectedCategoryId, int? selectedBrandId)
        {
            IQueryable<Product> products = _context.Products;
            if (selectedCategoryId != null)
            {
                products = products.Where(c=>c.CategoryId == selectedCategoryId);
            }
            if (selectedBrandId != null)
            {
                products = products.Where(b=>b.BrandId == selectedBrandId);
            }
            IEnumerable<Brand> brands = await _context.Brands.ToListAsync();
            IEnumerable<Category> categories =  await _context.Categories.ToListAsync();
            CreateImageVM createImageVM = new CreateImageVM()
            {
                Brands = new SelectList(brands, "Id", nameof(Brand.BrandName), selectedBrandId),
                Categories = new SelectList(categories, "Id", nameof(Category.CategoryName), selectedCategoryId),
                SelectedBrandId = selectedBrandId,
                SelectedCategoryId = selectedCategoryId,
                Products = new SelectList(products, "Id", nameof(Product.ProductName))
            };
            return View(createImageVM);
        }

        // POST: ProductImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateImageVM crImageVm)
        {
            if (ModelState.IsValid)
            {
                foreach (IFormFile file in crImageVm.Photos)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        ProductImage image = new ProductImage
                        {
                            ImageData = ms.ToArray(),
                            ProductId = crImageVm.SelectedProductId
                        };
                        _context.Images.Add(image);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            IQueryable<Product> products = _context.Products;
            if (crImageVm.SelectedCategoryId != null)
                products = products.Where(p => p.CategoryId == crImageVm.SelectedCategoryId);
            if (crImageVm.SelectedBrandId != null)
                products = products.Where(p => p.BrandId == crImageVm.SelectedBrandId);
            IEnumerable<Brand> brands = await _context.Brands.ToListAsync();
            IEnumerable<Category> categories = await _context.Categories.ToListAsync();
            crImageVm.Brands = new SelectList(brands, "Id", nameof(Brand.BrandName),
                crImageVm.SelectedBrandId);
            crImageVm.Categories = new SelectList(categories, "Id",
               nameof(Category.CategoryName), crImageVm.SelectedCategoryId);
            crImageVm.Products = new SelectList(products, "Id",
                nameof(Product.ProductName), crImageVm.SelectedProductId);

            return View(crImageVm);
        }

        // GET: ProductImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImage = await _context.Images
                 .Include(p => p.Product)
               .ThenInclude(p => p.ProductImages)
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (productImage == null)
            {
                return NotFound();
            }
            ProductImageDTO productImgDTO = _mapper.Map<ProductImageDTO>(productImage);
            ViewBag.ProductId = new SelectList(_context.Products, "Id", "ProductName", productImage.ProductId);
            return View(productImgDTO);
        }

        // POST: ProductImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductImageDTO productImage, IFormFile foto)
        {
            if (id != productImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var prodIm = await _context.Images
                        .Include(i => i.Product) 
                        .FirstOrDefaultAsync(i => i.Id == id);

                    if (prodIm == null)
                    {
                        return NotFound();
                    }

                    if (productImage.Product != null)
                    {
                        prodIm.Product.ProductName = productImage.Product.ProductName;
                        _context.Update(prodIm.Product); 
                    }
                    if (foto != null)
                    {
                        using MemoryStream ms = new MemoryStream();
                        await foto.CopyToAsync(ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        prodIm.ImageData = ms.ToArray();
                    }
                   
                    _context.Update(prodIm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductImageExists(productImage.Id))
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

            ViewBag.ProductId = new SelectList(_context.Products, "Id", "ProductName", productImage.ProductId);

            return View(productImage);
        }

        // GET: ProductImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImage = await _context.Images
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productImage == null)
            {
                return NotFound();
            }

            return View(productImage);
        }

        // POST: ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productImage = await _context.Images.FindAsync(id);
            if (productImage != null)
            {
                _context.Images.Remove(productImage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductImageExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }

        public async Task<IActionResult> GetProducts(int? brandId, int? categoryId)
        {
            IEnumerable<Brand> brands = await _context.Brands.ToListAsync();
            IEnumerable<Category> categories = await _context.Categories.ToListAsync();
            IQueryable<Product> products = _context.Products;
            if (categoryId != null)
                products = products.Where(p => p.CategoryId == categoryId);
            if (brandId != null)
                products = products.Where(p => p.BrandId == brandId);
            CreateImageVM vM = new CreateImageVM
            {
                Brands = new SelectList(brands, "Id", nameof(Brand.BrandName), brandId),
                Categories = new SelectList(categories, "Id",
                nameof(Category.CategoryName), categoryId),
                SelectedBrandId = brandId,
                SelectedCategoryId = categoryId,
                Products = new SelectList(products, "Id",
                nameof(Product.ProductName)),
            };
            return PartialView("_SelectProductBlock", vM);
        }

    }
}
