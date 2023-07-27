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
    public class OrdreCreationRTQsController : Controller
    {
        private readonly GAPContext _context;

        public OrdreCreationRTQsController(GAPContext context)
        {
            _context = context;
        }

        // GET: OrdreCreationRTQs
        public async Task<IActionResult> Index()
        {
            var gAPContext = _context.OrdreCreationRTQ.Include(o => o.Devis);
            return View(await gAPContext.ToListAsync());
        }

        // GET: OrdreCreationRTQs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrdreCreationRTQ == null)
            {
                return NotFound();
            }

            var ordreCreationRTQ = await _context.OrdreCreationRTQ
                .Include(o => o.Devis)
                .FirstOrDefaultAsync(m => m.OrdreCreationRTQID == id);
            if (ordreCreationRTQ == null)
            {
                return NotFound();
            }

            return View(ordreCreationRTQ);
        }

        // GET: OrdreCreationRTQs/Create
        public IActionResult Create()
        {
            ViewData["DevisID"] = new SelectList(_context.Devis, "DevisID", "DevisID");
            return View();
        }

        // POST: OrdreCreationRTQs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrdreCreationRTQID,DevisID,RespServiceQualiteId")] OrdreCreationRTQ ordreCreationRTQ)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordreCreationRTQ);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DevisID"] = new SelectList(_context.Devis, "DevisID", "DevisID", ordreCreationRTQ.DevisID);
            return View(ordreCreationRTQ);
        }

        // GET: OrdreCreationRTQs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrdreCreationRTQ == null)
            {
                return NotFound();
            }

            var ordreCreationRTQ = await _context.OrdreCreationRTQ.FindAsync(id);
            if (ordreCreationRTQ == null)
            {
                return NotFound();
            }
            ViewData["DevisID"] = new SelectList(_context.Devis, "DevisID", "DevisID", ordreCreationRTQ.DevisID);
            return View(ordreCreationRTQ);
        }

        // POST: OrdreCreationRTQs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrdreCreationRTQID,DevisID,RespServiceQualiteId")] OrdreCreationRTQ ordreCreationRTQ)
        {
            if (id != ordreCreationRTQ.OrdreCreationRTQID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordreCreationRTQ);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdreCreationRTQExists(ordreCreationRTQ.OrdreCreationRTQID))
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
            ViewData["DevisID"] = new SelectList(_context.Devis, "DevisID", "DevisID", ordreCreationRTQ.DevisID);
            return View(ordreCreationRTQ);
        }

        // GET: OrdreCreationRTQs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrdreCreationRTQ == null)
            {
                return NotFound();
            }

            var ordreCreationRTQ = await _context.OrdreCreationRTQ
                .Include(o => o.Devis)
                .FirstOrDefaultAsync(m => m.OrdreCreationRTQID == id);
            if (ordreCreationRTQ == null)
            {
                return NotFound();
            }

            return View(ordreCreationRTQ);
        }

        // POST: OrdreCreationRTQs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrdreCreationRTQ == null)
            {
                return Problem("Entity set 'GAPContext.OrdreCreationRTQ'  is null.");
            }
            var ordreCreationRTQ = await _context.OrdreCreationRTQ.FindAsync(id);
            if (ordreCreationRTQ != null)
            {
                _context.OrdreCreationRTQ.Remove(ordreCreationRTQ);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdreCreationRTQExists(int id)
        {
          return (_context.OrdreCreationRTQ?.Any(e => e.OrdreCreationRTQID == id)).GetValueOrDefault();
        }
    }
}
