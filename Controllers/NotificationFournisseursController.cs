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
    public class NotificationFournisseursController : Controller
    {
        private readonly GAPContext _context;

        public NotificationFournisseursController(GAPContext context)
        {
            _context = context;
        }

        // GET: NotificationFournisseurs
        public async Task<IActionResult> Index()
        {
              return _context.NotificationFournisseur != null ? 
                          View(await _context.NotificationFournisseur.ToListAsync()) :
                          Problem("Entity set 'GAPContext.NotificationFournisseur'  is null.");
        }

        // GET: NotificationFournisseurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NotificationFournisseur == null)
            {
                return NotFound();
            }

            var notificationFournisseur = await _context.NotificationFournisseur
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notificationFournisseur == null)
            {
                return NotFound();
            }

            return View(notificationFournisseur);
        }

        // GET: NotificationFournisseurs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NotificationFournisseurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FournisseurID,OffreVenteID,NotificationID,NotificationTitle")] NotificationFournisseur notificationFournisseur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notificationFournisseur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notificationFournisseur);
        }

        // GET: NotificationFournisseurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NotificationFournisseur == null)
            {
                return NotFound();
            }

            var notificationFournisseur = await _context.NotificationFournisseur.FindAsync(id);
            if (notificationFournisseur == null)
            {
                return NotFound();
            }
            return View(notificationFournisseur);
        }

        // POST: NotificationFournisseurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FournisseurID,OffreVenteID,NotificationID,NotificationTitle")] NotificationFournisseur notificationFournisseur)
        {
            if (id != notificationFournisseur.NotificationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificationFournisseur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationFournisseurExists(notificationFournisseur.NotificationID))
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
            return View(notificationFournisseur);
        }

        // GET: NotificationFournisseurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NotificationFournisseur == null)
            {
                return NotFound();
            }

            var notificationFournisseur = await _context.NotificationFournisseur
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notificationFournisseur == null)
            {
                return NotFound();
            }

            return View(notificationFournisseur);
        }

        // POST: NotificationFournisseurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NotificationFournisseur == null)
            {
                return Problem("Entity set 'GAPContext.NotificationFournisseur'  is null.");
            }
            var notificationFournisseur = await _context.NotificationFournisseur.FindAsync(id);
            if (notificationFournisseur != null)
            {
                _context.NotificationFournisseur.Remove(notificationFournisseur);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationFournisseurExists(int id)
        {
          return (_context.NotificationFournisseur?.Any(e => e.NotificationID == id)).GetValueOrDefault();
        }
    }
}
