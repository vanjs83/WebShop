using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Controllers
{
    [Authorize(Roles ="Admin")]
    public class MeasuringUnitController : Controller
    {
        private readonly DbShop _context;

        public MeasuringUnitController(DbShop context)
        {
            _context = context;
        }

        // GET: MeasuringUnit
        public async Task<IActionResult> Index()
        {
            return View(await _context.MeasuringUnit.ToListAsync());
        }

        // GET: MeasuringUnit/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measuringUnit = await _context.MeasuringUnit
                .FirstOrDefaultAsync(m => m.MeasuringUnitId == id);
            if (measuringUnit == null)
            {
                return NotFound();
            }

            return View(measuringUnit);
        }

        // GET: MeasuringUnit/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MeasuringUnit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MeasuringUnitId,Name,Unit,Description")] MeasuringUnit measuringUnit)
        {

            measuringUnit.CreatedBy = User.Identity.Name;
            measuringUnit.ModifiedBy = User.Identity.Name;
            measuringUnit.CreatedDate = DateTime.Today;
            measuringUnit.ModifiedDate = DateTime.Today;

            if (ModelState.IsValid)
            {
                measuringUnit.MeasuringUnitId = Guid.NewGuid();
                _context.Add(measuringUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(measuringUnit);
        }

        // GET: MeasuringUnit/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measuringUnit = await _context.MeasuringUnit.FindAsync(id);
            if (measuringUnit == null)
            {
                return NotFound();
            }
            return View(measuringUnit);
        }

        // POST: MeasuringUnit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MeasuringUnitId,Name,Unit,Description")] MeasuringUnit measuringUnit)
        {

            measuringUnit.ModifiedBy = User.Identity.Name;
            measuringUnit.ModifiedDate = DateTime.Today;

            if (id != measuringUnit.MeasuringUnitId)
            {
                return NotFound();
            }

            measuringUnit.ModifiedDate = DateTime.Today;
            measuringUnit.ModifiedBy = User.Identity.Name;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(measuringUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeasuringUnitExists(measuringUnit.MeasuringUnitId))
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
            return View(measuringUnit);
        }

        // GET: MeasuringUnit/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measuringUnit = await _context.MeasuringUnit
                .FirstOrDefaultAsync(m => m.MeasuringUnitId == id);
            if (measuringUnit == null)
            {
                return NotFound();
            }

            return View(measuringUnit);
        }

        // POST: MeasuringUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var measuringUnit = await _context.MeasuringUnit.FindAsync(id);
            _context.MeasuringUnit.Remove(measuringUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeasuringUnitExists(Guid id)
        {
            return _context.MeasuringUnit.Any(e => e.MeasuringUnitId == id);
        }
    }
}
