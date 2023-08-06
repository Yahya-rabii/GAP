using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using X.PagedList;

namespace GAP.Controllers
{
    public class ReceptionReportsController : Controller
    {
        private readonly GAPContext _context;

        public ReceptionReportsController(GAPContext context)
        {
            _context = context;
        }

        // GET: ReceptionReports
        public async Task<IActionResult> Index(int? page)
        {
            IQueryable<ReceptionReport> iseriq = from rc in _context.ReceptionReport
                                                  select rc;


            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));


            
        }

        // GET: ReceptionReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReceptionReport == null)
            {
                return NotFound();
            }

            var ReceptionReport = await _context.ReceptionReport
                .FirstOrDefaultAsync(m => m.ReceptionReportID == id);
            if (ReceptionReport == null)
            {
                return NotFound();
            }

            return View(ReceptionReport);
        }

        // GET: ReceptionReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReceptionReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReceptionReportID,CreationDate,PurchasingReceptionistId,PurchaseQuoteId")] ReceptionReport ReceptionReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ReceptionReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ReceptionReport);
        }

        // GET: ReceptionReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReceptionReport == null)
            {
                return NotFound();
            }

            var ReceptionReport = await _context.ReceptionReport.FindAsync(id);
            if (ReceptionReport == null)
            {
                return NotFound();
            }
            return View(ReceptionReport);
        }

        // POST: ReceptionReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReceptionReportID,CreationDate,PurchasingReceptionistId,PurchaseQuoteId")] ReceptionReport ReceptionReport)
        {
            if (id != ReceptionReport.ReceptionReportID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ReceptionReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceptionReportExists(ReceptionReport.ReceptionReportID))
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
            return View(ReceptionReport);
        }

        // GET: ReceptionReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReceptionReport == null)
            {
                return NotFound();
            }

            var ReceptionReport = await _context.ReceptionReport
                .FirstOrDefaultAsync(m => m.ReceptionReportID == id);
            if (ReceptionReport == null)
            {
                return NotFound();
            }

            return View(ReceptionReport);
        }

        // POST: ReceptionReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReceptionReport == null)
            {
                return Problem("Entity set 'GAPContext.ReceptionReport'  is null.");
            }
            var ReceptionReport = await _context.ReceptionReport.FindAsync(id);
            if (ReceptionReport != null)
            {
                _context.ReceptionReport.Remove(ReceptionReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceptionReportExists(int id)
        {
          return (_context.ReceptionReport?.Any(e => e.ReceptionReportID == id)).GetValueOrDefault();
        }
    }
}
