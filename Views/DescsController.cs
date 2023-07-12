using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;

namespace GAP.Views
{
    public class DescsController : Controller
    {
        private readonly GAPContext _context;

        public DescsController(GAPContext context)
        {
            _context = context;
        }

        // GET: Descs
        public async Task<IActionResult> Index()
        {
              return _context.Desc != null ? 
                          View(await _context.Desc.ToListAsync()) :
                          Problem("Entity set 'GAPContext.Desc'  is null.");
        }

        // GET: Descs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Desc == null)
            {
                return NotFound();
            }

            var desc = await _context.Desc
                .FirstOrDefaultAsync(m => m.Id == id);
            if (desc == null)
            {
                return NotFound();
            }

            return View(desc);
        }

        // GET: Descs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Descs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,NombrePiece,Description")] Desc desc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(desc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(desc);
        }

        // GET: Descs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Desc == null)
            {
                return NotFound();
            }

            var desc = await _context.Desc.FindAsync(id);
            if (desc == null)
            {
                return NotFound();
            }
            return View(desc);
        }

        // POST: Descs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,NombrePiece,Description")] Desc desc)
        {
            if (id != desc.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(desc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DescExists(desc.Id))
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
            return View(desc);
        }

        // GET: Descs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Desc == null)
            {
                return NotFound();
            }

            var desc = await _context.Desc
                .FirstOrDefaultAsync(m => m.Id == id);
            if (desc == null)
            {
                return NotFound();
            }

            return View(desc);
        }

        // POST: Descs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Desc == null)
            {
                return Problem("Entity set 'GAPContext.Desc'  is null.");
            }
            var desc = await _context.Desc.FindAsync(id);
            if (desc != null)
            {
                _context.Desc.Remove(desc);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DescExists(int id)
        {
          return (_context.Desc?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
