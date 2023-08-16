using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using X.PagedList;

namespace GAP.Controllers
{
    public class SanctionsController : Controller
    {
        private readonly GAPContext _context;

        public SanctionsController(GAPContext context)
        {
            _context = context;
        }

        // GET: Sanctions
        public async Task<IActionResult> Index(string SearchString, int? page)
        {
            IQueryable<Sanction> iseriq = from s in _context.Sanction
                                      select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                iseriq = _context.Sanction.Where(s => s.SupplierId == _context.Supplier.Where(ss => ss.Email.ToLower().Contains(SearchString.ToLower().Trim())).FirstOrDefault().UserID) ;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: Sanctions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sanction == null)
            {
                return NotFound();
            }

            var sanction = await _context.Sanction
                .FirstOrDefaultAsync(m => m.SanctionID == id);
            if (sanction == null)
            {
                return NotFound();
            }

            return View(sanction);
        }

        // GET: Sanctions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sanctions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SanctionID,SanctionTitle,SanctionDescription,SupplierId")] Sanction sanction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sanction);
        }

        // GET: Sanctions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sanction == null)
            {
                return NotFound();
            }

            var sanction = await _context.Sanction.FindAsync(id);
            if (sanction == null)
            {
                return NotFound();
            }
            return View(sanction);
        }

        // POST: Sanctions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SanctionID,SanctionTitle,SanctionDescription,SupplierId")] Sanction sanction)
        {
            if (id != sanction.SanctionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanctionExists(sanction.SanctionID))
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
            return View(sanction);
        }

        // GET: Sanctions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sanction == null)
            {
                return NotFound();
            }

            var sanction = await _context.Sanction
                .FirstOrDefaultAsync(m => m.SanctionID == id);
            if (sanction == null)
            {
                return NotFound();
            }

            return View(sanction);
        }

        // POST: Sanctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sanction == null)
            {
                return Problem("Entity set 'GAPContext.Sanction'  is null.");
            }
            var sanction = await _context.Sanction.FindAsync(id);
            if (sanction != null)
            {
                _context.Sanction.Remove(sanction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanctionExists(int id)
        {
          return (_context.Sanction?.Any(e => e.SanctionID == id)).GetValueOrDefault();
        }
    }
}
