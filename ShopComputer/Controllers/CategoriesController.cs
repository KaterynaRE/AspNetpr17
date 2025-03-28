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
using ShopComputer.Models.ViewModels.Categories;
using ShopComputerDomainLibrary;

namespace ShopComputer.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ShopContext _context;
        private readonly IMapper mapper;

        public CategoriesController(ShopContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var shopContext = _context.Categories.Include(c => c.ParentCategory);
            return View(await shopContext.ToListAsync());
        }


        // GET: Categories/Details/5
        [Authorize(Roles = "manager")]
        [Authorize(Policy = "managerPolicy")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        //public IActionResult Create()
        //{
        //    ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
        //    return View();
        //}

        [Authorize(Roles = "manager")]
        [Authorize(Policy = "managerPolicy")]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories
                .Include(c=>c.ParentCategory)
                .ToListAsync();
            CreateCategoryVM vM = new CreateCategoryVM()
            {
                ParentCategories = new SelectList(categories, "Id", "CategoryName")
            };
            return View(vM);
        }


        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryVM vM, IList<int?> parentCategoryId)
        {
            if (ModelState.IsValid)
            {
                parentCategoryId = parentCategoryId.Where(t => t.HasValue).ToList();
                Category category = mapper.Map<Category>(vM.CategoryDTO);
                if (parentCategoryId.Count() > 0)
                    category.ParentCategoryId = parentCategoryId[parentCategoryId.Count() - 1];
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vM.ParentCategories = new SelectList(_context.Categories, "Id",
                "CategoryName", vM.CategoryDTO.ParentCategoryId);
            return View(vM);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "manager")]
        [Authorize(Policy = "managerPolicy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryName,ParentCategoryId")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "manager")]
        [Authorize(Policy = "managerPolicy")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        public async Task<IActionResult> GetChildCategories(int? id)
        {
            if (id == null)
                return NotFound();
            var categories = await _context.Categories
                .Where(c => c.ParentCategoryId == id)
                .ToListAsync();
            if (!categories.Any())
                return NotFound();
            return PartialView("_ChildCategories", categories);
        }



    }
}
