using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
namespace GAP.Controllers
{

    [Authorize]
    [Authorize(Roles = "RespServiceAchat,Fournisseur")]
    public class DemandeAchatsController : Controller
    {
        private readonly GAPContext _context;

        public DemandeAchatsController(GAPContext context)
        {
            _context = context;
        }

        // GET: DemandeAchats
        public async Task<IActionResult> Index()
        {
              return _context.DemandeAchat != null ? 
                          View(await _context.DemandeAchat.ToListAsync()) :
                          Problem("Entity set 'GAPContext.DemandeAchat'  is null.");
        }

        // GET: DemandeAchats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DemandeAchat == null)
            {
                return NotFound();
            }

            var demandeAchat = await _context.DemandeAchat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (demandeAchat == null)
            {
                return NotFound();
            }

            return View(demandeAchat);
        }

        // GET: DemandeAchats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DemandeAchats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Budget")] DemandeAchat demandeAchat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(demandeAchat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(demandeAchat);
        }

        // GET: DemandeAchats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DemandeAchat == null)
            {
                return NotFound();
            }

            var demandeAchat = await _context.DemandeAchat.FindAsync(id);
            if (demandeAchat == null)
            {
                return NotFound();
            }
            return View(demandeAchat);
        }

        // POST: DemandeAchats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Budget")] DemandeAchat demandeAchat)
        {
            if (id != demandeAchat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(demandeAchat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DemandeAchatExists(demandeAchat.Id))
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
            return View(demandeAchat);
        }

        // GET: DemandeAchats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DemandeAchat == null)
            {
                return NotFound();
            }

            var demandeAchat = await _context.DemandeAchat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (demandeAchat == null)
            {
                return NotFound();
            }

            return View(demandeAchat);
        }

        // POST: DemandeAchats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DemandeAchat == null)
            {
                return Problem("Entity set 'GAPContext.DemandeAchat'  is null.");
            }
            var demandeAchat = await _context.DemandeAchat.FindAsync(id);
            if (demandeAchat != null)
            {
                _context.DemandeAchat.Remove(demandeAchat);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DemandeAchatExists(int id)
        {
          return (_context.DemandeAchat?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
