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
    public class ReclamationsHistoriesController : Controller
    {
        private readonly GAPContext _context;

        public ReclamationsHistoriesController(GAPContext context)
        {
            _context = context;
        }

        // GET: ReclamationsHistories
        public async Task<IActionResult> Index()
        {
              return _context.ReclamationsHistory != null ? 
                          View(await _context.ReclamationsHistory.ToListAsync()) :
                          Problem("Entity set 'GAPContext.ReclamationsHistory'  is null.");
        }

        // GET: ReclamationsHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReclamationsHistory == null)
            {
                return NotFound();
            }

            var reclamationsHistory = await _context.ReclamationsHistory
                .FirstOrDefaultAsync(m => m.ReclamationsHistoryID == id);
            if (reclamationsHistory == null)
            {
                return NotFound();
            }

            return View(reclamationsHistory);
        }

        // GET: ReclamationsHistories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReclamationsHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReclamationsHistoryID,ReclamationsIDS")] ReclamationsHistory reclamationsHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reclamationsHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reclamationsHistory);
        }

        // GET: ReclamationsHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReclamationsHistory == null)
            {
                return NotFound();
            }

            var reclamationsHistory = await _context.ReclamationsHistory.FindAsync(id);
            if (reclamationsHistory == null)
            {
                return NotFound();
            }
            return View(reclamationsHistory);
        }

        // POST: ReclamationsHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReclamationsHistoryID,ReclamationsIDS")] ReclamationsHistory reclamationsHistory)
        {
            if (id != reclamationsHistory.ReclamationsHistoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reclamationsHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReclamationsHistoryExists(reclamationsHistory.ReclamationsHistoryID))
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
            return View(reclamationsHistory);
        }

        // GET: ReclamationsHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReclamationsHistory == null)
            {
                return NotFound();
            }

            var reclamationsHistory = await _context.ReclamationsHistory
                .FirstOrDefaultAsync(m => m.ReclamationsHistoryID == id);
            if (reclamationsHistory == null)
            {
                return NotFound();
            }

            return View(reclamationsHistory);
        }

        // POST: ReclamationsHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReclamationsHistory == null)
            {
                return Problem("Entity set 'GAPContext.ReclamationsHistory'  is null.");
            }
            var reclamationsHistory = await _context.ReclamationsHistory.FindAsync(id);
            if (reclamationsHistory != null)
            {
                _context.ReclamationsHistory.Remove(reclamationsHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReclamationsHistoryExists(int id)
        {
          return (_context.ReclamationsHistory?.Any(e => e.ReclamationsHistoryID == id)).GetValueOrDefault();
        }
    }
}
