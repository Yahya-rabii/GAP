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
    public class OrdreCreationFacturesController : Controller
    {
        private readonly GAPContext _context;

        public OrdreCreationFacturesController(GAPContext context)
        {
            _context = context;
        }

        // GET: OrdreCreationFactures
        public async Task<IActionResult> Index()
        {
            var gAPContext = _context.OrdreCreationFacture.Include(o => o.Devis);
            return View(await gAPContext.ToListAsync());
        }

        // GET: OrdreCreationFactures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrdreCreationFacture == null)
            {
                return NotFound();
            }

            var ordreCreationFacture = await _context.OrdreCreationFacture
                .Include(o => o.Devis)
                .FirstOrDefaultAsync(m => m.OrdreCreationFactureID == id);
            if (ordreCreationFacture == null)
            {
                return NotFound();
            }

            return View(ordreCreationFacture);
        }

        // GET: OrdreCreationFactures/Create
        public IActionResult Create()
        {
            ViewData["DevisID"] = new SelectList(_context.Devis, "DevisID", "DevisID");
            return View();
        }

        // POST: OrdreCreationFactures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrdreCreationFactureID,DevisID,RespServiceFinanceId")] OrdreCreationFacture ordreCreationFacture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordreCreationFacture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DevisID"] = new SelectList(_context.Devis, "DevisID", "DevisID", ordreCreationFacture.DevisID);
            return View(ordreCreationFacture);
        }

        // GET: OrdreCreationFactures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrdreCreationFacture == null)
            {
                return NotFound();
            }

            var ordreCreationFacture = await _context.OrdreCreationFacture.FindAsync(id);
            if (ordreCreationFacture == null)
            {
                return NotFound();
            }
            ViewData["DevisID"] = new SelectList(_context.Devis, "DevisID", "DevisID", ordreCreationFacture.DevisID);
            return View(ordreCreationFacture);
        }

        // POST: OrdreCreationFactures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrdreCreationFactureID,DevisID,RespServiceFinanceId")] OrdreCreationFacture ordreCreationFacture)
        {
            if (id != ordreCreationFacture.OrdreCreationFactureID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordreCreationFacture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdreCreationFactureExists(ordreCreationFacture.OrdreCreationFactureID))
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
            ViewData["DevisID"] = new SelectList(_context.Devis, "DevisID", "DevisID", ordreCreationFacture.DevisID);
            return View(ordreCreationFacture);
        }

        // GET: OrdreCreationFactures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrdreCreationFacture == null)
            {
                return NotFound();
            }

            var ordreCreationFacture = await _context.OrdreCreationFacture
                .Include(o => o.Devis)
                .FirstOrDefaultAsync(m => m.OrdreCreationFactureID == id);
            if (ordreCreationFacture == null)
            {
                return NotFound();
            }

            return View(ordreCreationFacture);
        }

        // POST: OrdreCreationFactures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrdreCreationFacture == null)
            {
                return Problem("Entity set 'GAPContext.OrdreCreationFacture'  is null.");
            }
            var ordreCreationFacture = await _context.OrdreCreationFacture.FindAsync(id);
            if (ordreCreationFacture != null)
            {
                _context.OrdreCreationFacture.Remove(ordreCreationFacture);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdreCreationFactureExists(int id)
        {
          return (_context.OrdreCreationFacture?.Any(e => e.OrdreCreationFactureID == id)).GetValueOrDefault();
        }
    }
}
