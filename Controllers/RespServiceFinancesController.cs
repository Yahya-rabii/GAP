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
    public class RespServiceFinancesController : Controller
    {
        private readonly GAPContext _context;

        public RespServiceFinancesController(GAPContext context)
        {
            _context = context;
        }

        // GET: RespServiceFinances
        public async Task<IActionResult> Index()
        {
              return _context.RespServiceFinance != null ? 
                          View(await _context.RespServiceFinance.ToListAsync()) :
                          Problem("Entity set 'GAPContext.RespServiceFinance'  is null.");
        }

        // GET: RespServiceFinances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RespServiceFinance == null)
            {
                return NotFound();
            }

            var respServiceFinance = await _context.RespServiceFinance
                .FirstOrDefaultAsync(m => m.RespServiceFinanceID == id);
            if (respServiceFinance == null)
            {
                return NotFound();
            }

            return View(respServiceFinance);
        }

        // GET: RespServiceFinances/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RespServiceFinances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RespServiceFinanceID,Email,Password,FirstName,LastName")] RespServiceFinance respServiceFinance)
        {
            if (ModelState.IsValid)
            {

                bool userExists = await _context.HistoryU.AnyAsync(r => r.Email == respServiceFinance.Email);
                if (userExists)
                {
                    ModelState.AddModelError("Email", "respServiceFinance with this email already exists.");
                    return View(respServiceFinance);
                }



                RespServiceFinance respServiceFinance1 = new(

             respServiceFinance.RespServiceFinanceID,
             respServiceFinance.Email,
             respServiceFinance.Password,
             respServiceFinance.FirstName,
             respServiceFinance.LastName

             );
                HistoryU historyU = new(
                 respServiceFinance.RespServiceFinanceID,
                 respServiceFinance.Email,
                 "respServiceFinance"

                 );

                _context.HistoryU.Add(historyU);

                _context.Add(respServiceFinance1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(respServiceFinance);
        }

        // GET: RespServiceFinances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RespServiceFinance == null)
            {
                return NotFound();
            }

            var respServiceFinance = await _context.RespServiceFinance.FindAsync(id);
            if (respServiceFinance == null)
            {
                return NotFound();
            }
            return View(respServiceFinance);
        }

        // POST: RespServiceFinances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RespServiceFinanceID,Email,Password,FirstName,LastName")] RespServiceFinance respServiceFinance)
        {
            if (id != respServiceFinance.RespServiceFinanceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(respServiceFinance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RespServiceFinanceExists(respServiceFinance.RespServiceFinanceID))
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
            return View(respServiceFinance);
        }

        // GET: RespServiceFinances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RespServiceFinance == null)
            {
                return NotFound();
            }

            var respServiceFinance = await _context.RespServiceFinance
                .FirstOrDefaultAsync(m => m.RespServiceFinanceID == id);
            if (respServiceFinance == null)
            {
                return NotFound();
            }

            return View(respServiceFinance);
        }

        // POST: RespServiceFinances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RespServiceFinance == null)
            {
                return Problem("Entity set 'GAPContext.RespServiceFinance'  is null.");
            }
            var respServiceFinance = await _context.RespServiceFinance.FindAsync(id);
            if (respServiceFinance != null)
            {
                _context.RespServiceFinance.Remove(respServiceFinance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RespServiceFinanceExists(int id)
        {
          return (_context.RespServiceFinance?.Any(e => e.RespServiceFinanceID == id)).GetValueOrDefault();
        }
    }
}
