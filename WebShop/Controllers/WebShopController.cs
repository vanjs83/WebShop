using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop.Common;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Controllers
{
    [AllowAnonymous]
    public class WebShopController : Controller
    {
        private readonly DbShop _context;

        public WebShopController(DbShop context)
        {
            _context = context;
        }

        // GET: WebShop
        public async Task<IActionResult> Index()
        {

            var productCategory = await _context.ProductCategory.Include(x => x.Product).Include(x => x.Category).ToListAsync();


          //  var dbShop = _context.Product.Include(p => p.MeasuringUnit);
            
            return View(productCategory);
        }

        [HttpGet]
        public ActionResult Review(ReviewContent review)
        {

            try
            {
                review.ReviewContentId = Guid.NewGuid();
                review.CreatedDate = DateTime.Today;

                _context.ReviewContent.Add(review);
                _context.SaveChanges();
                
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return Json("ok");
        }


        [HttpGet()]
        [ActionName("Reviews")]
       
        public PartialViewResult Review(Guid productId)
        {
            if(productId == null)
            {
                Response.StatusCode = 405;
            }

            List<ReviewContent> reviews = _context.ReviewContent.Where(x => x.ProductId == productId).ToList();
            

            return PartialView("_Reviews", reviews);
        }


        public async Task<IActionResult> Product(int currentPage = 1)
        {
            //  var productCategory = await _context.ProductCategory.Include(x => x.Product).Include(x => x.Category).ToListAsync();
            ModelPaging productCategory = new ModelPaging(_context);
            productCategory.CurrentPage = currentPage;
            productCategory.Data = await _context.ProductCategory.Include(x => x.Product).Include(x => x.Category).Skip(currentPage - 1).Take(1).ToListAsync();

            //  var dbShop = _context.Product.Include(p => p.MeasuringUnit);

            return View(productCategory);
        }


        [HttpGet]
        public PartialViewResult TechSpec(Guid id)
        {
            if (id == null)
            {
                Response.StatusCode = 405;
            }


            string tehnSpec = " Technical Specification!!";
            return PartialView("_TechSpec", tehnSpec);

        }


        [HttpGet]
        public PartialViewResult Geometry(Guid? id)
        {
            if(id == null)
            {
                Response.StatusCode = 405;
            }


            string geo = "Not specifield yet!!";
            return PartialView("_Geometry", geo);
        }



        [HttpGet]
        public PartialViewResult Description(Guid? id)
        {
            string desc = string.Empty;
            try
            {
                desc =   _context.Product.FirstOrDefault(x => x.ProductId == id).Description;
            }
            catch (Exception ex)
            {
                desc =ex.Message;
            }
            return  PartialView("_Description", desc);
        }


        // GET: WebShop/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product prod = await _context.Product.Include(x => x.MeasuringUnit).FirstOrDefaultAsync(x => x.ProductId == id);
             
            
            
            ProductCategory product = await _context.ProductCategory.Include(x => x.Product).Include(x => x.Category).FirstOrDefaultAsync(x => x.Product.ProductId == id);
            product.Product = prod;
            product.ProductId = prod.ProductId;


            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: WebShop/Create
        public IActionResult Create()
        {
            ViewData["MeasuringUnitId"] = new SelectList(_context.MeasuringUnit, "MeasuringUnitId", "Name");
            return View();
        }

        // POST: WebShop/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,CreatedDate,ModifiedDate,Quantity,Available,Price,Image,MeasuringUnitId")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.ProductId = Guid.NewGuid();
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MeasuringUnitId"] = new SelectList(_context.MeasuringUnit, "MeasuringUnitId", "Name", product.MeasuringUnitId);
            return View(product);
        }

        // GET: WebShop/Edit/5
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
            ViewData["MeasuringUnitId"] = new SelectList(_context.MeasuringUnit, "MeasuringUnitId", "Name", product.MeasuringUnitId);
            return View(product);
        }

        // POST: WebShop/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProductId,Name,CreatedDate,ModifiedDate,Quantity,Available,Price,Image,MeasuringUnitId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
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
            ViewData["MeasuringUnitId"] = new SelectList(_context.MeasuringUnit, "MeasuringUnitId", "Name", product.MeasuringUnitId);
            return View(product);
        }

        // GET: WebShop/Delete/5
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

        // POST: WebShop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
