using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly DbShop _context;

        public ProductCategoryController(DbShop context)
        {
            _context = context;
        }

        // GET: ProductCategory
        public async Task<IActionResult> Index()
        {
            var dbShop = _context.ProductCategory.Include(p => p.Category).Include(p => p.Product);
            return View(await dbShop.ToListAsync());
        }

        // GET: ProductCategory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategory
                .Include(p => p.Category)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductCategoryId == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // GET: ProductCategory/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name");
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name");
            return View(new ProductCategory());
        }

        // POST: ProductCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductCategoryId,ProductId,CategoryId")] ProductCategory productCategory)
        {
            productCategory.CreatedBy = User.Identity.Name;
            productCategory.ModifiedBy = User.Identity.Name;
            productCategory.ModifiedDate = DateTime.Today;
            productCategory.CreatedDate = DateTime.Today;

            if (ModelState.IsValid)
            {
                productCategory.ProductCategoryId = Guid.NewGuid();
                _context.Add(productCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", productCategory.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name", productCategory.ProductId);
            return View(productCategory);
        }

        // GET: ProductCategory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategory.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", productCategory.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name", productCategory.ProductId);
            return View(productCategory);
        }

        // POST: ProductCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProductCategoryId,CreatedDate,CreatedBy,ProductId,CategoryId")] ProductCategory productCategory)
        {

            productCategory.ModifiedBy = User.Identity.Name;
            productCategory.ModifiedDate = DateTime.Today;


            if (id != productCategory.ProductCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoryExists(productCategory.ProductCategoryId))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", productCategory.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name", productCategory.ProductId);
            return View(productCategory);
        }

        // GET: ProductCategory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategory
                .Include(p => p.Category)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductCategoryId == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // POST: ProductCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var productCategory = await _context.ProductCategory.FindAsync(id);
            _context.ProductCategory.Remove(productCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCategoryExists(Guid id)
        {
            return _context.ProductCategory.Any(e => e.ProductCategoryId == id);
        }
    }
}
