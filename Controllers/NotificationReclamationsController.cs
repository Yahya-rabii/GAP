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
    public class NotificationReclamationsController : Controller
    {
        private readonly GAPContext _context;

        public NotificationReclamationsController(GAPContext context)
        {
            _context = context;
        }

        // GET: NotificationReclamations
        public async Task<IActionResult> Index()
        {
              return _context.NotificationReclamation != null ? 
                          View(await _context.NotificationReclamation.ToListAsync()) :
                          Problem("Entity set 'GAPContext.NotificationReclamation'  is null.");
        }

        // GET: NotificationReclamations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NotificationReclamation == null)
            {
                return NotFound();
            }

            var notificationReclamation = await _context.NotificationReclamation
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notificationReclamation == null)
            {
                return NotFound();
            }

            return View(notificationReclamation);
        }

        // GET: NotificationReclamations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NotificationReclamations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NotificationID,NotificationTitle")] NotificationReclamation notificationReclamation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notificationReclamation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notificationReclamation);
        }

        // GET: NotificationReclamations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NotificationReclamation == null)
            {
                return NotFound();
            }

            var notificationReclamation = await _context.NotificationReclamation.FindAsync(id);
            if (notificationReclamation == null)
            {
                return NotFound();
            }
            return View(notificationReclamation);
        }

        // POST: NotificationReclamations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NotificationID,NotificationTitle")] NotificationReclamation notificationReclamation)
        {
            if (id != notificationReclamation.NotificationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificationReclamation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationReclamationExists(notificationReclamation.NotificationID))
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
            return View(notificationReclamation);
        }

        // GET: NotificationReclamations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NotificationReclamation == null)
            {
                return NotFound();
            }

            var notificationReclamation = await _context.NotificationReclamation
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notificationReclamation == null)
            {
                return NotFound();
            }

            return View(notificationReclamation);
        }

        // POST: NotificationReclamations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NotificationReclamation == null)
            {
                return Problem("Entity set 'GAPContext.NotificationReclamation'  is null.");
            }
            var notificationReclamation = await _context.NotificationReclamation.FindAsync(id);
            if (notificationReclamation != null)
            {
                _context.NotificationReclamation.Remove(notificationReclamation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationReclamationExists(int id)
        {
          return (_context.NotificationReclamation?.Any(e => e.NotificationID == id)).GetValueOrDefault();
        }
    }
}
