using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Helper;

namespace GAP.Controllers
{
    public class HistoryUsController : Controller
    {
        private readonly GAPContext _context;

        public HistoryUsController(GAPContext context)
        {
            _context = context;
        }

        // GET: HistoryUs
        public async Task<IActionResult> Index()
        {
              return _context.HistoryU != null ? 
                          View(await _context.HistoryU.ToListAsync()) :
                          Problem("Entity set 'GAPContext.HistoryU'  is null.");
        }

        // GET: HistoryUs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HistoryU == null)
            {
                return NotFound();
            }

            var historyU = await _context.HistoryU
                .FirstOrDefaultAsync(m => m.HistoryUID == id);
            if (historyU == null)
            {
                return NotFound();
            }

            return View(historyU);
        }

        // GET: HistoryUs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HistoryUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HistoryUID,Email,Titulair")] HistoryU historyU)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historyU);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(historyU);
        }

        // GET: HistoryUs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HistoryU == null)
            {
                return NotFound();
            }

            var historyU = await _context.HistoryU.FindAsync(id);
            if (historyU == null)
            {
                return NotFound();
            }
            return View(historyU);
        }

        // POST: HistoryUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HistoryUID,Email,Titulair")] HistoryU historyU)
        {
            if (id != historyU.HistoryUID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historyU);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoryUExists(historyU.HistoryUID))
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
            return View(historyU);
        }

        // GET: HistoryUs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HistoryU == null)
            {
                return NotFound();
            }

            var historyU = await _context.HistoryU
                .FirstOrDefaultAsync(m => m.HistoryUID == id);
            if (historyU == null)
            {
                return NotFound();
            }

            return View(historyU);
        }

        // POST: HistoryUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HistoryU == null)
            {
                return Problem("Entity set 'GAPContext.HistoryU'  is null.");
            }
            var historyU = await _context.HistoryU.FindAsync(id);
            if (historyU != null)
            {
                _context.HistoryU.Remove(historyU);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoryUExists(int id)
        {
          return (_context.HistoryU?.Any(e => e.HistoryUID == id)).GetValueOrDefault();
        }
    }
}
