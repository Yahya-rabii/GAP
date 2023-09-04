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
    public class BillPurchasesController : Controller
    {
        private readonly GAPContext _context;

        public BillPurchasesController(GAPContext context)
        {
            _context = context;
        }

        // GET: BillPurchases
        public async Task<IActionResult> Index()
        {
            var gAPContext = _context.BillPurchase;
            return View(await gAPContext.ToListAsync());
        }

        // GET: BillPurchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BillPurchase == null)
            {
                return NotFound();
            }

            var billPurchase = await _context.BillPurchase
                .FirstOrDefaultAsync(m => m.BillID == id);
            if (billPurchase == null)
            {
                return NotFound();
            }

            return View(billPurchase);
        }

        // GET: BillPurchases/Create
        public IActionResult Create()
        {
            ViewData["PurchaseQuoteID"] = new SelectList(_context.PurchaseQuote, "PurchaseQuoteID", "PurchaseQuoteID");
            return View();
        }

        // POST: BillPurchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseQuoteID,BillID,Validity,FinanceDepartmentManagerId")] BillPurchase billPurchase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(billPurchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PurchaseQuoteID"] = new SelectList(_context.PurchaseQuote, "PurchaseQuoteID", "PurchaseQuoteID", billPurchase.PurchaseQuoteID);
            return View(billPurchase);
        }

        // GET: BillPurchases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BillPurchase == null)
            {
                return NotFound();
            }

            var billPurchase = await _context.BillPurchase.FindAsync(id);
            if (billPurchase == null)
            {
                return NotFound();
            }
            ViewData["PurchaseQuoteID"] = new SelectList(_context.PurchaseQuote, "PurchaseQuoteID", "PurchaseQuoteID", billPurchase.PurchaseQuoteID);
            return View(billPurchase);
        }

        // POST: BillPurchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PurchaseQuoteID,BillID,Validity,FinanceDepartmentManagerId")] BillPurchase billPurchase)
        {
            if (id != billPurchase.BillID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billPurchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillPurchaseExists(billPurchase.BillID))
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
            ViewData["PurchaseQuoteID"] = new SelectList(_context.PurchaseQuote, "PurchaseQuoteID", "PurchaseQuoteID", billPurchase.PurchaseQuoteID);
            return View(billPurchase);
        }

        // GET: BillPurchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BillPurchase == null)
            {
                return NotFound();
            }

            var billPurchase = await _context.BillPurchase
                .FirstOrDefaultAsync(m => m.BillID == id);
            if (billPurchase == null)
            {
                return NotFound();
            }

            return View(billPurchase);
        }

        // POST: BillPurchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BillPurchase == null)
            {
                return Problem("Entity set 'GAPContext.BillPurchase'  is null.");
            }
            var billPurchase = await _context.BillPurchase.FindAsync(id);
            if (billPurchase != null)
            {
                _context.BillPurchase.Remove(billPurchase);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillPurchaseExists(int id)
        {
          return (_context.BillPurchase?.Any(e => e.BillID == id)).GetValueOrDefault();
        }
    }
}
