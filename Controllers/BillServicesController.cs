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
    public class BillServicesController : Controller
    {
        private readonly GAPContext _context;

        public BillServicesController(GAPContext context)
        {
            _context = context;
        }

        // GET: BillServices
        public async Task<IActionResult> Index()
        {
            var gAPContext = _context.BillService;
            return View(await gAPContext.ToListAsync());
        }

        // GET: BillServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BillService == null)
            {
                return NotFound();
            }

            var billService = await _context.BillService
                .FirstOrDefaultAsync(m => m.BillID == id);
            if (billService == null)
            {
                return NotFound();
            }

            return View(billService);
        }

        // GET: BillServices/Create
        public IActionResult Create()
        {
            ViewData["ServiceQuoteID"] = new SelectList(_context.ServiceQuote, "ServiceQuoteID", "ServiceQuoteID");
            return View();
        }

        // POST: BillServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceQuoteID,BillID,Validity,FinanceDepartmentManagerId")] BillService billService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(billService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceQuoteID"] = new SelectList(_context.ServiceQuote, "ServiceQuoteID", "ServiceQuoteID", billService.ServiceQuoteID);
            return View(billService);
        }

        // GET: BillServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BillService == null)
            {
                return NotFound();
            }

            var billService = await _context.BillService.FindAsync(id);
            if (billService == null)
            {
                return NotFound();
            }
            ViewData["ServiceQuoteID"] = new SelectList(_context.ServiceQuote, "ServiceQuoteID", "ServiceQuoteID", billService.ServiceQuoteID);
            return View(billService);
        }

        // POST: BillServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceQuoteID,BillID,Validity,FinanceDepartmentManagerId")] BillService billService)
        {
            if (id != billService.BillID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillServiceExists(billService.BillID))
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
            ViewData["ServiceQuoteID"] = new SelectList(_context.ServiceQuote, "ServiceQuoteID", "ServiceQuoteID", billService.ServiceQuoteID);
            return View(billService);
        }

        // GET: BillServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BillService == null)
            {
                return NotFound();
            }

            var billService = await _context.BillService
                .FirstOrDefaultAsync(m => m.BillID == id);
            if (billService == null)
            {
                return NotFound();
            }

            return View(billService);
        }

        // POST: BillServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BillService == null)
            {
                return Problem("Entity set 'GAPContext.BillService'  is null.");
            }
            var billService = await _context.BillService.FindAsync(id);
            if (billService != null)
            {
                _context.BillService.Remove(billService);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillServiceExists(int id)
        {
          return (_context.BillService?.Any(e => e.BillID == id)).GetValueOrDefault();
        }
    }
}
