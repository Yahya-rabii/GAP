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
    public class DevisController : Controller
    {
        private readonly GAPContext _context;

        public DevisController(GAPContext context)
        {
            _context = context;
        }

        // GET: Devis
        public async Task<IActionResult> Index()
        {
              return _context.Devis != null ? 
                          View(await _context.Devis.ToListAsync()) :
                          Problem("Entity set 'GAPContext.Devis'  is null.");
        }

        // GET: Devis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Devis == null)
            {
                return NotFound();
            }

            var devis = await _context.Devis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (devis == null)
            {
                return NotFound();
            }

            return View(devis);
        }

        // GET: Devis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Devis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RespServiceAchatId,Id,DateCreation,PrixTTL,DateReception,NombrePiece")] Devis devis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(devis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(devis);
        }

        // GET: Devis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Devis == null)
            {
                return NotFound();
            }

            var devis = await _context.Devis.FindAsync(id);
            if (devis == null)
            {
                return NotFound();
            }
            return View(devis);
        }

        // POST: Devis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RespServiceAchatId,Id,DateCreation,PrixTTL,DateReception,NombrePiece")] Devis devis)
        {
            if (id != devis.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DevisExists(devis.Id))
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
            return View(devis);
        }

        // GET: Devis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Devis == null)
            {
                return NotFound();
            }

            var devis = await _context.Devis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (devis == null)
            {
                return NotFound();
            }

            return View(devis);
        }

        // POST: Devis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Devis == null)
            {
                return Problem("Entity set 'GAPContext.Devis'  is null.");
            }
            var devis = await _context.Devis.FindAsync(id);
            if (devis != null)
            {
                _context.Devis.Remove(devis);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DevisExists(int id)
        {
          return (_context.Devis?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
