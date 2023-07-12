using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;

namespace GAP.Controllers
{
    public class RapportReceptionsController : Controller
    {
        private readonly GAPContext _context;

        public RapportReceptionsController(GAPContext context)
        {
            _context = context;
        }

        // GET: RapportReceptions
        public async Task<IActionResult> Index()
        {
              return _context.RapportReception != null ? 
                          View(await _context.RapportReception.ToListAsync()) :
                          Problem("Entity set 'GAPContext.RapportReception'  is null.");
        }

        // GET: RapportReceptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RapportReception == null)
            {
                return NotFound();
            }

            var rapportReception = await _context.RapportReception
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rapportReception == null)
            {
                return NotFound();
            }

            return View(rapportReception);
        }

        // GET: RapportReceptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RapportReceptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date")] RapportReception rapportReception)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rapportReception);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rapportReception);
        }

        // GET: RapportReceptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RapportReception == null)
            {
                return NotFound();
            }

            var rapportReception = await _context.RapportReception.FindAsync(id);
            if (rapportReception == null)
            {
                return NotFound();
            }
            return View(rapportReception);
        }

        // POST: RapportReceptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date")] RapportReception rapportReception)
        {
            if (id != rapportReception.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rapportReception);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RapportReceptionExists(rapportReception.Id))
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
            return View(rapportReception);
        }

        // GET: RapportReceptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RapportReception == null)
            {
                return NotFound();
            }

            var rapportReception = await _context.RapportReception
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rapportReception == null)
            {
                return NotFound();
            }

            return View(rapportReception);
        }

        // POST: RapportReceptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RapportReception == null)
            {
                return Problem("Entity set 'GAPContext.RapportReception'  is null.");
            }
            var rapportReception = await _context.RapportReception.FindAsync(id);
            if (rapportReception != null)
            {
                _context.RapportReception.Remove(rapportReception);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RapportReceptionExists(int id)
        {
          return (_context.RapportReception?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
