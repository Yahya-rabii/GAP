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
using Swashbuckle.AspNetCore.Annotations;

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
        [HttpGet("/ReceptionReports")]
        [SwaggerOperation(Summary = "Get reception reports", Description = "Retrieve a list of reception reports.")]
        [SwaggerResponse(200, "List of reception reports retrieved successfully.")]
        public async Task<IActionResult> Index(int? page)
        {
            IQueryable<ReceptionReport> iseriq = from rc in _context.ReceptionReport
                                                  select rc;


            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));


            
        }






        // GET: ReceptionReports/Details/5
        [HttpGet("/ReceptionReports/Details/{id}")]
        [SwaggerOperation(Summary = "Get details of a reception report", Description = "Retrieve the details of a reception report.")]
        [SwaggerResponse(200, "Reception report details retrieved successfully.")]
        [SwaggerResponse(404, "Reception report not found.")]
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
        [HttpGet("/ReceptionReports/Create")]
        [SwaggerOperation(Summary = "Show reception report creation form", Description = "Display the reception report creation form.")]
        [SwaggerResponse(200, "Reception report creation form displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }





        // POST: ReceptionReports/Create
        [HttpPost("/ReceptionReports/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new reception report", Description = "Create a new reception report with the provided information.")]
        [SwaggerResponse(200, "Reception report created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
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
        [HttpGet("/ReceptionReports/Edit/{id}")]
        [SwaggerOperation(Summary = "Show reception report edit form", Description = "Display the reception report edit form.")]
        [SwaggerResponse(200, "Reception report edit form displayed successfully.")]
        [SwaggerResponse(404, "Reception report not found.")]
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
        [HttpPost("/ReceptionReports/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit a reception report", Description = "Edit an existing reception report with the provided information.")]
        [SwaggerResponse(200, "Reception report edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Reception report not found.")]
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
        [HttpGet("/ReceptionReports/Delete/{id}")]
        [SwaggerOperation(Summary = "Show reception report delete confirmation", Description = "Display the reception report delete confirmation.")]
        [SwaggerResponse(200, "Reception report delete confirmation displayed successfully.")]
        [SwaggerResponse(404, "Reception report not found.")]
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
        [HttpPost("/ReceptionReports/Delete/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete a reception report", Description = "Delete an existing reception report.")]
        [SwaggerResponse(200, "Reception report deleted successfully.")]
        [SwaggerResponse(404, "Reception report not found.")]
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








        /*---------------------------------------------------------------*/





        // Helper: no route
        private bool ReceptionReportExists(int id)
        {
          return (_context.ReceptionReport?.Any(e => e.ReceptionReportID == id)).GetValueOrDefault();
        }
    }
}
