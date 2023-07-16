using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using GAP.Helper;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace GAP.Controllers
{
    [Authorize]
    [Authorize(Roles = "Admin")]
    public class RespServiceQualitesController : Controller
    {
        private readonly GAPContext _context;

        public RespServiceQualitesController(GAPContext context)
        {
            _context = context;
        }

        // GET: RespServiceQualites
        public async Task<IActionResult> Index()
        {
              return _context.RespServiceQualite != null ? 
                          View(await _context.RespServiceQualite.ToListAsync()) :
                          Problem("Entity set 'GAPContext.RespServiceQualite'  is null.");
        }

        // GET: RespServiceQualites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RespServiceQualite == null)
            {
                return NotFound();
            }

            var respServiceQualite = await _context.RespServiceQualite
                .FirstOrDefaultAsync(m => m.RespServiceQualiteID == id);
            if (respServiceQualite == null)
            {
                return NotFound();
            }

            return View(respServiceQualite);
        }

        // GET: RespServiceQualites/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RespServiceQualites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RespServiceQualiteID,Email,Password,FirstName,LastName")] RespServiceQualite respServiceQualite)
        {
            if (ModelState.IsValid)
            {


                bool userExists = await _context.HistoryU.AnyAsync(r => r.Email == respServiceQualite.Email);
                if (userExists)
                {
                    ModelState.AddModelError("Email", "respServiceQualite with this email already exists.");
                    return View(respServiceQualite);
                }


                RespServiceQualite respServiceQualite1 = new(

        respServiceQualite.RespServiceQualiteID,
        respServiceQualite.Email,
        respServiceQualite.Password,
        respServiceQualite.FirstName,
        respServiceQualite.LastName

        );

                HistoryU historyU = new(
                respServiceQualite.RespServiceQualiteID,
                respServiceQualite.Email,
                "respServiceQualite"

                );

                _context.HistoryU.Add(historyU);

                _context.Add(respServiceQualite1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(respServiceQualite);
        }

        // GET: RespServiceQualites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RespServiceQualite == null)
            {
                return NotFound();
            }

            var respServiceQualite = await _context.RespServiceQualite.FindAsync(id);
            if (respServiceQualite == null)
            {
                return NotFound();
            }
            return View(respServiceQualite);
        }

        // POST: RespServiceQualites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RespServiceQualiteID,Email,Password,FirstName,LastName")] RespServiceQualite respServiceQualite)
        {
            if (id != respServiceQualite.RespServiceQualiteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(respServiceQualite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RespServiceQualiteExists(respServiceQualite.RespServiceQualiteID))
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
            return View(respServiceQualite);
        }

        // GET: RespServiceQualites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RespServiceQualite == null)
            {
                return NotFound();
            }

            var respServiceQualite = await _context.RespServiceQualite
                .FirstOrDefaultAsync(m => m.RespServiceQualiteID == id);
            if (respServiceQualite == null)
            {
                return NotFound();
            }

            return View(respServiceQualite);
        }

        // POST: RespServiceQualites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RespServiceQualite == null)
            {
                return Problem("Entity set 'GAPContext.RespServiceQualite'  is null.");
            }
            var respServiceQualite = await _context.RespServiceQualite.FindAsync(id);
            if (respServiceQualite != null)
            {
                _context.RespServiceQualite.Remove(respServiceQualite);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RespServiceQualiteExists(int id)
        {
          return (_context.RespServiceQualite?.Any(e => e.RespServiceQualiteID == id)).GetValueOrDefault();
        }
    }
}
