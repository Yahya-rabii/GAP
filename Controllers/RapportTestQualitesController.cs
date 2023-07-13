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
    public class RapportTestQualitesController : Controller
    {
        private readonly GAPContext _context;

        public RapportTestQualitesController(GAPContext context)
        {
            _context = context;
        }

        // GET: RapportTestQualites
        public async Task<IActionResult> Index()
        {
            var gAPContext = _context.RapportTestQualite.Include(r => r.RespServiceQualite);
            return View(await gAPContext.ToListAsync());
        }

        // GET: RapportTestQualites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RapportTestQualite == null)
            {
                return NotFound();
            }

            var rapportTestQualite = await _context.RapportTestQualite
                .Include(r => r.RespServiceQualite)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rapportTestQualite == null)
            {
                return NotFound();
            }

            return View(rapportTestQualite);
        }

        // GET: RapportTestQualites/Create
        public IActionResult Create()
        {
            ViewData["RespServiceQualiteId"] = new SelectList(_context.RespServiceQualite, "RespServiceQualiteID", "Email");
            return View();
        }

        // POST: RapportTestQualites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ValiditeEtat,ValiditeNbrPiece,ValiditeFonctionnement,RespServiceQualiteId")] RapportTestQualite rapportTestQualite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rapportTestQualite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RespServiceQualiteId"] = new SelectList(_context.RespServiceQualite, "RespServiceQualiteID", "Email", rapportTestQualite.RespServiceQualiteId);
            return View(rapportTestQualite);
        }

        // GET: RapportTestQualites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RapportTestQualite == null)
            {
                return NotFound();
            }

            var rapportTestQualite = await _context.RapportTestQualite.FindAsync(id);
            if (rapportTestQualite == null)
            {
                return NotFound();
            }
            ViewData["RespServiceQualiteId"] = new SelectList(_context.RespServiceQualite, "RespServiceQualiteID", "Email", rapportTestQualite.RespServiceQualiteId);
            return View(rapportTestQualite);
        }

        // POST: RapportTestQualites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ValiditeEtat,ValiditeNbrPiece,ValiditeFonctionnement,RespServiceQualiteId")] RapportTestQualite rapportTestQualite)
        {
            if (id != rapportTestQualite.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rapportTestQualite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RapportTestQualiteExists(rapportTestQualite.Id))
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
            ViewData["RespServiceQualiteId"] = new SelectList(_context.RespServiceQualite, "RespServiceQualiteID", "Email", rapportTestQualite.RespServiceQualiteId);
            return View(rapportTestQualite);
        }

        // GET: RapportTestQualites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RapportTestQualite == null)
            {
                return NotFound();
            }

            var rapportTestQualite = await _context.RapportTestQualite
                .Include(r => r.RespServiceQualite)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rapportTestQualite == null)
            {
                return NotFound();
            }

            return View(rapportTestQualite);
        }

        // POST: RapportTestQualites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RapportTestQualite == null)
            {
                return Problem("Entity set 'GAPContext.RapportTestQualite'  is null.");
            }
            var rapportTestQualite = await _context.RapportTestQualite.FindAsync(id);
            if (rapportTestQualite != null)
            {
                _context.RapportTestQualite.Remove(rapportTestQualite);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RapportTestQualiteExists(int id)
        {
          return (_context.RapportTestQualite?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
