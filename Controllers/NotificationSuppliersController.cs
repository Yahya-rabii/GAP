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
    public class NotificationSuppliersController : Controller
    {
        private readonly GAPContext _context;

        public NotificationSuppliersController(GAPContext context)
        {
            _context = context;
        }

        // GET: NotificationSuppliers
        public async Task<IActionResult> Index()
        {
              return _context.NotificationSupplier != null ? 
                          View(await _context.NotificationSupplier.ToListAsync()) :
                          Problem("Entity set 'GAPContext.NotificationSupplier'  is null.");
        }

        // GET: NotificationSuppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NotificationSupplier == null)
            {
                return NotFound();
            }

            var notificationSupplier = await _context.NotificationSupplier
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notificationSupplier == null)
            {
                return NotFound();
            }

            return View(notificationSupplier);
        }

        // GET: NotificationSuppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NotificationSuppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierID,SaleOfferID,NotificationID,NotificationTitle")] NotificationSupplier notificationSupplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notificationSupplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notificationSupplier);
        }

        // GET: NotificationSuppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NotificationSupplier == null)
            {
                return NotFound();
            }

            var notificationSupplier = await _context.NotificationSupplier.FindAsync(id);
            if (notificationSupplier == null)
            {
                return NotFound();
            }
            return View(notificationSupplier);
        }

        // POST: NotificationSuppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierID,SaleOfferID,NotificationID,NotificationTitle")] NotificationSupplier notificationSupplier)
        {
            if (id != notificationSupplier.NotificationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificationSupplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationSupplierExists(notificationSupplier.NotificationID))
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
            return View(notificationSupplier);
        }

        // GET: NotificationSuppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NotificationSupplier == null)
            {
                return NotFound();
            }

            var notificationSupplier = await _context.NotificationSupplier
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notificationSupplier == null)
            {
                return NotFound();
            }

            return View(notificationSupplier);
        }

        // POST: NotificationSuppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NotificationSupplier == null)
            {
                return Problem("Entity set 'GAPContext.NotificationSupplier'  is null.");
            }
            var notificationSupplier = await _context.NotificationSupplier.FindAsync(id);
            if (notificationSupplier != null)
            {
                _context.NotificationSupplier.Remove(notificationSupplier);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationSupplierExists(int id)
        {
          return (_context.NotificationSupplier?.Any(e => e.NotificationID == id)).GetValueOrDefault();
        }
    }
}
