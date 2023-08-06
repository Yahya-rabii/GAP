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
    public class NotificationInfosController : Controller
    {
        private readonly GAPContext _context;

        public NotificationInfosController(GAPContext context)
        {
            _context = context;
        }

        // GET: NotificationInfos
        public async Task<IActionResult> Index()
        {
              return _context.NotificationInfo != null ? 
                          View(await _context.NotificationInfo.ToListAsync()) :
                          Problem("Entity set 'GAPContext.NotificationInfo'  is null.");
        }

        // GET: NotificationInfos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NotificationInfo == null)
            {
                return NotFound();
            }

            var NotificationInfo = await _context.NotificationInfo
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (NotificationInfo == null)
            {
                return NotFound();
            }

            return View(NotificationInfo);
        }

        // GET: NotificationInfos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NotificationInfos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,PurchaseQuoteID,NotificationID,NotificationTitle")] NotificationInfo NotificationInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(NotificationInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(NotificationInfo);
        }

        // GET: NotificationInfos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NotificationInfo == null)
            {
                return NotFound();
            }

            var NotificationInfo = await _context.NotificationInfo.FindAsync(id);
            if (NotificationInfo == null)
            {
                return NotFound();
            }
            return View(NotificationInfo);
        }

        // POST: NotificationInfos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,PurchaseQuoteID,NotificationID,NotificationTitle")] NotificationInfo NotificationInfo)
        {
            if (id != NotificationInfo.NotificationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(NotificationInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationInfoExists(NotificationInfo.NotificationID))
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
            return View(NotificationInfo);
        }

        // GET: NotificationInfos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NotificationInfo == null)
            {
                return NotFound();
            }

            var NotificationInfo = await _context.NotificationInfo
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (NotificationInfo == null)
            {
                return NotFound();
            }

            return View(NotificationInfo);
        }

        // POST: NotificationInfos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NotificationInfo == null)
            {
                return Problem("Entity set 'GAPContext.NotificationInfo'  is null.");
            }
            var NotificationInfo = await _context.NotificationInfo.FindAsync(id);
            if (NotificationInfo != null)
            {
                _context.NotificationInfo.Remove(NotificationInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationInfoExists(int id)
        {
          return (_context.NotificationInfo?.Any(e => e.NotificationID == id)).GetValueOrDefault();
        }
    }
}
