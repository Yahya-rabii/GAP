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
    public class RespServiceAchatsController : Controller
    {
        private readonly GAPContext _context;

        public RespServiceAchatsController(GAPContext context)
        {
            _context = context;
        }

        // GET: RespServiceAchats
        public async Task<IActionResult> Index()
        {
              return _context.RespServiceAchat != null ? 
                          View(await _context.RespServiceAchat.ToListAsync()) :
                          Problem("Entity set 'GAPContext.RespServiceAchat'  is null.");
        }

        // GET: RespServiceAchats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RespServiceAchat == null)
            {
                return NotFound();
            }

            var respServiceAchat = await _context.RespServiceAchat
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (respServiceAchat == null)
            {
                return NotFound();
            }

            return View(respServiceAchat);
        }

        // GET: RespServiceAchats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RespServiceAchats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,Tutulaire")] RespServiceAchat respServiceAchat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(respServiceAchat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(respServiceAchat);
        }

        // GET: RespServiceAchats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RespServiceAchat == null)
            {
                return NotFound();
            }

            var respServiceAchat = await _context.RespServiceAchat.FindAsync(id);
            if (respServiceAchat == null)
            {
                return NotFound();
            }
            return View(respServiceAchat);
        }

        // POST: RespServiceAchats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,FirstName,LastName,Tutulaire")] RespServiceAchat respServiceAchat)
        {
            if (id != respServiceAchat.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(respServiceAchat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RespServiceAchatExists(respServiceAchat.UserID))
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
            return View(respServiceAchat);
        }

        // GET: RespServiceAchats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RespServiceAchat == null)
            {
                return NotFound();
            }

            var respServiceAchat = await _context.RespServiceAchat
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (respServiceAchat == null)
            {
                return NotFound();
            }

            return View(respServiceAchat);
        }

        // POST: RespServiceAchats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RespServiceAchat == null)
            {
                return Problem("Entity set 'GAPContext.RespServiceAchat'  is null.");
            }
            var respServiceAchat = await _context.RespServiceAchat.FindAsync(id);
            if (respServiceAchat != null)
            {
                _context.RespServiceAchat.Remove(respServiceAchat);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RespServiceAchatExists(int id)
        {
          return (_context.RespServiceAchat?.Any(e => e.UserID == id)).GetValueOrDefault();
        }
    }
}
