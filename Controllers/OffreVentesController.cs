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
    public class OffreVentesController : Controller
    {
        private readonly GAPContext _context;

        public OffreVentesController(GAPContext context)
        {
            _context = context;
        }

        // GET: OffreVentes
        public async Task<IActionResult> Index()
        {
            var gAPContext = _context.OffreVente.Include(o => o.Provider);
            return View(await gAPContext.ToListAsync());
        }

        // GET: OffreVentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OffreVente == null)
            {
                return NotFound();
            }

            var offreVente = await _context.OffreVente
                .Include(o => o.Provider)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offreVente == null)
            {
                return NotFound();
            }

            return View(offreVente);
        }

        // GET: OffreVentes/Create
        public IActionResult Create()
        {
            ViewData["FournisseurId"] = new SelectList(_context.Fournisseur, "Id", "Email");
            return View();
        }

        // POST: OffreVentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FournisseurId,Id,PrixTTL,Validite")] OffreVente offreVente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(offreVente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FournisseurId"] = new SelectList(_context.Fournisseur, "Id", "Email", offreVente.FournisseurId);
            return View(offreVente);
        }

        // GET: OffreVentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OffreVente == null)
            {
                return NotFound();
            }

            var offreVente = await _context.OffreVente.FindAsync(id);
            if (offreVente == null)
            {
                return NotFound();
            }
            ViewData["FournisseurId"] = new SelectList(_context.Fournisseur, "Id", "Email", offreVente.FournisseurId);
            return View(offreVente);
        }

        // POST: OffreVentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FournisseurId,Id,PrixTTL,Validite")] OffreVente offreVente)
        {
            if (id != offreVente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offreVente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OffreVenteExists(offreVente.Id))
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
            ViewData["FournisseurId"] = new SelectList(_context.Fournisseur, "Id", "Email", offreVente.FournisseurId);
            return View(offreVente);
        }

        // GET: OffreVentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OffreVente == null)
            {
                return NotFound();
            }

            var offreVente = await _context.OffreVente
                .Include(o => o.Provider)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offreVente == null)
            {
                return NotFound();
            }

            return View(offreVente);
        }

        // POST: OffreVentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OffreVente == null)
            {
                return Problem("Entity set 'GAPContext.OffreVente'  is null.");
            }
            var offreVente = await _context.OffreVente.FindAsync(id);
            if (offreVente != null)
            {
                _context.OffreVente.Remove(offreVente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OffreVenteExists(int id)
        {
          return (_context.OffreVente?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
