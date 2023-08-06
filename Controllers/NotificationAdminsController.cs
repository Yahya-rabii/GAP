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
    public class NotificationAdminsController : Controller
    {
        private readonly GAPContext _context;

        public NotificationAdminsController(GAPContext context)
        {
            _context = context;
        }

        // GET: NotificationAdmins
        public async Task<IActionResult> Index()
        {
              return _context.NotificationAdmin != null ? 
                          View(await _context.NotificationAdmin.ToListAsync()) :
                          Problem("Entity set 'GAPContext.NotificationAdmin'  is null.");
        }

        // GET: NotificationAdmins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NotificationAdmin == null)
            {
                return NotFound();
            }

            var notificationAdmin = await _context.NotificationAdmin
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notificationAdmin == null)
            {
                return NotFound();
            }

            return View(notificationAdmin);
        }

        // GET: NotificationAdmins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NotificationAdmins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierID,NotificationID,NotificationTitle")] NotificationAdmin notificationAdmin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notificationAdmin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notificationAdmin);
        }

        // GET: NotificationAdmins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NotificationAdmin == null)
            {
                return NotFound();
            }

            var notificationAdmin = await _context.NotificationAdmin.FindAsync(id);
            if (notificationAdmin == null)
            {
                return NotFound();
            }
            return View(notificationAdmin);
        }

        // POST: NotificationAdmins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierID,NotificationID,NotificationTitle")] NotificationAdmin notificationAdmin)
        {
            if (id != notificationAdmin.NotificationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificationAdmin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationAdminExists(notificationAdmin.NotificationID))
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
            return View(notificationAdmin);
        }

        // GET: NotificationAdmins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NotificationAdmin == null)
            {
                return NotFound();
            }

            var notificationAdmin = await _context.NotificationAdmin
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notificationAdmin == null)
            {
                return NotFound();
            }

            return View(notificationAdmin);
        }

        // POST: NotificationAdmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NotificationAdmin == null)
            {
                return Problem("Entity set 'GAPContext.NotificationAdmin'  is null.");
            }
            var notificationAdmin = await _context.NotificationAdmin.FindAsync(id);
            if (notificationAdmin != null)
            {
                _context.NotificationAdmin.Remove(notificationAdmin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationAdminExists(int id)
        {
          return (_context.NotificationAdmin?.Any(e => e.NotificationID == id)).GetValueOrDefault();
        }
    }
}
