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
    public class FournisseursController : Controller
    {
        private readonly GAPContext _context;

        public FournisseursController(GAPContext context)
        {
            _context = context;
        }

        // GET: Fournisseurs
        public async Task<IActionResult> Index()
        {
              return _context.Fournisseur != null ? 
                          View(await _context.Fournisseur.ToListAsync()) :
                          Problem("Entity set 'GAPContext.Fournisseur'  is null.");
        }

        // GET: Fournisseurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Fournisseur == null)
            {
                return NotFound();
            }

            var fournisseur = await _context.Fournisseur
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fournisseur == null)
            {
                return NotFound();
            }

            return View(fournisseur);
        }

        // GET: Fournisseurs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fournisseurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Password,Id,Nom,NombreTransaction")] Fournisseur fournisseur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fournisseur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fournisseur);
        }

        // GET: Fournisseurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Fournisseur == null)
            {
                return NotFound();
            }

            var fournisseur = await _context.Fournisseur.FindAsync(id);
            if (fournisseur == null)
            {
                return NotFound();
            }
            return View(fournisseur);
        }

        // POST: Fournisseurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Email,Password,Id,Nom,NombreTransaction")] Fournisseur fournisseur)
        {
            if (id != fournisseur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fournisseur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FournisseurExists(fournisseur.Id))
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
            return View(fournisseur);
        }

        // GET: Fournisseurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fournisseur == null)
            {
                return NotFound();
            }

            var fournisseur = await _context.Fournisseur
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fournisseur == null)
            {
                return NotFound();
            }

            return View(fournisseur);
        }

        // POST: Fournisseurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fournisseur == null)
            {
                return Problem("Entity set 'GAPContext.Fournisseur'  is null.");
            }
            var fournisseur = await _context.Fournisseur.FindAsync(id);
            if (fournisseur != null)
            {
                _context.Fournisseur.Remove(fournisseur);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FournisseurExists(int id)
        {
          return (_context.Fournisseur?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
