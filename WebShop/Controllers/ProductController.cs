using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebShop.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly DbShop _context;

        public ProductController(DbShop context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var product = await (from ca in _context.ProductCategory
                           join p in _context.Product on ca.ProductId equals p.ProductId
                           join cc in _context.Category on ca.CategoryId equals cc.CategoryId
                           select ca).ToListAsync();

            var prCat = await _context.ProductCategory.Include(x => x.Product).Include(x => x.Category).ToListAsync();


            var dbShop = _context.Product.Include(p => p.MeasuringUnit);
            return View(await dbShop.ToListAsync());

            
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.MeasuringUnit)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["MeasuringUnitId"] = new SelectList(_context.MeasuringUnit, "MeasuringUnitId", "Unit");
            return View(new Product());
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("ProductId,Name,CreatedDate,ModifiedDate,Quantity,Available,Price,Image,Description,MeasuringUnitId")]  Product product)
        {


            if (Request.Form.Files["Image"] != null)
            {
                IFormFile file = Request.Form.Files["Image"];
                var stream = file.OpenReadStream();
                BinaryReader br = new BinaryReader(stream);
                byte[] imgdata = br.ReadBytes((int)stream.Length);
              product.Image = imgdata;
            }  



            if (ModelState.IsValid)
            {
              
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MeasuringUnitId"] = new SelectList(_context.MeasuringUnit, "MeasuringUnitId", "Unit", product.MeasuringUnitId);
            return View(product);
        }


        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["MeasuringUnitId"] = new SelectList(_context.MeasuringUnit, "MeasuringUnitId", "Unit", product.MeasuringUnitId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProductId,Name,CreatedDate,ModifiedDate,Quantity,Available,Price,Image,Description,MeasuringUnitId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }


            if (Request.Form.Files["Image"] != null)
            {
                IFormFile file = Request.Form.Files["Image"];
                var stream = file.OpenReadStream();
                BinaryReader br = new BinaryReader(stream);
                byte[] imgdata = br.ReadBytes((int)stream.Length);
                product.Image = imgdata;
            }
            else
            {
                var image = _context.Product.AsNoTracking().FirstOrDefault(x => x.ProductId == product.ProductId).Image;
                product.Image = image;
                _context.Entry(product).State = EntityState.Detached;
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(product).State = EntityState.Modified;
                   // _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["MeasuringUnitId"] = new SelectList(_context.MeasuringUnit, "MeasuringUnitId", "Unit", product.MeasuringUnitId);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.MeasuringUnit)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                Product product = await _context.Product.FindAsync(id);
                if (product != null)
                {
                    List<ProductCategory> productCategories = await _context.ProductCategory.Where(x => x.ProductId == id).ToListAsync();
                    if (productCategories.Any())
                    {
                        _context.ProductCategory.RemoveRange(productCategories);
                        await _context.SaveChangesAsync();
                    }
                    List<OrderDetails> orderDetails = await _context.OrderDetail.Where(x => x.ProductId == id).ToListAsync();
                    if (orderDetails.Any())
                    {
                        _context.OrderDetail.RemoveRange(orderDetails);
                        await _context.SaveChangesAsync();
                    }

                }

                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
               
            }
            catch(Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
