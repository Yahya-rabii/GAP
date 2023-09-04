using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using System.Security.Claims;
using X.PagedList;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Controllers
{
    public class QualityTestReportsController : Controller
    {
        private readonly GAPContext _context;

        public QualityTestReportsController(GAPContext context)
        {
            _context = context;
        }



        // GET: QualityTestReports/Index
        [HttpGet("/QualityTestReports")]
        [SwaggerOperation(Summary = "Get quality test reports", Description = "Retrieve a list of quality test reports.")]
        [SwaggerResponse(200, "List of quality test reports retrieved successfully.")]
        public async Task<IActionResult> Index(int? page)
        {
            IQueryable<QualityTestReport> QualityTestReports = from rq in _context.QualityTestReport
                                                                 select rq;

            var data = await _context.PurchaseQuote
                .Where(d => QualityTestReports.Any(rq => d.PurchaseQuoteID == rq.PurchaseQuoteId))
                .Select(PurchaseQuote => new
                {
                    SupplierEmail = PurchaseQuote.Supplier.Email,
                    Products = PurchaseQuote.Products,
                    // Add other properties you need
                })
                .ToListAsync();

            ViewBag.RapportData = data;

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(await QualityTestReports.ToPagedListAsync(pageNumber, pageSize));
        }








        // GET: QualityTestReports/Details/5
        [HttpGet("/QualityTestReports/Details/{id}")]
        [SwaggerOperation(Summary = "Get details of a quality test report", Description = "Retrieve the details of a quality test report.")]
        [SwaggerResponse(200, "Quality test report details retrieved successfully.")]
        [SwaggerResponse(404, "Quality test report not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.QualityTestReport == null)
            {
                return NotFound();
            }

            var QualityTestReport = await _context.QualityTestReport
                .FirstOrDefaultAsync(m => m.QualityTestReportID == id);
            if (QualityTestReport == null)
            {
                return NotFound();
            }

            return View(QualityTestReport);
        }





        // GET: QualityTestReport/Create
        [HttpGet("/QualityTestReports/Create")]
        [SwaggerOperation(Summary = "Show quality test report creation form", Description = "Display the quality test report creation form.")]
        [SwaggerResponse(200, "Quality test report creation form displayed successfully.")]
        public IActionResult Create(int PurchaseQuoteId , int ServiceQuoteId)
        {
            

           
            ViewBag.PurchaseQuoteId = PurchaseQuoteId;
           
            ViewBag.ServiceQuoteId = ServiceQuoteId;

            return View();
        }





        // POST: QualityTestReport/Create
        [HttpPost("/QualityTestReports/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new quality test report", Description = "Create a new quality test report with the provided information.")]
        [SwaggerResponse(200, "Quality test report created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public IActionResult Create(QualityTestReport QualityTestReport, int PurchaseQuoteId, int ServiceQuoteId)
        {
           

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);



                if (PurchaseQuoteId != 0)
                {


                    QualityTestReport.PurchaseQuoteId = PurchaseQuoteId;
                    QualityTestReport.QualityTestingDepartmentManagerId = userId;


                    if (QualityTestReport.CntItemsValidity && QualityTestReport.StateValidity && QualityTestReport.OperationValidity)
                    {

                        var PurchaseQuote = _context.PurchaseQuote.Where(f => f.PurchaseQuoteID == PurchaseQuoteId).FirstOrDefault();
                        var Bill = _context.BillPurchase.Where(f => f.PurchaseQuoteID == PurchaseQuoteId).FirstOrDefault();
                        var Supplier = _context.Supplier.Where(f => f.UserID == PurchaseQuote.SupplierID).FirstOrDefault();
                        var saleOffer = _context.SaleOffer.Include(so => so.Products).Where(so => so.SupplierId == Supplier.UserID).FirstOrDefault();
                        var Products = saleOffer.Products.ToList();

                        var stock = new Stock();
                        if (Bill != null && Supplier != null)
                        {
                            Bill.Validity = true;
                            Supplier.TransactionNumber++;
                            foreach (var product in Products)
                            {
                                stock.Products.Add(product);

                            }

                            _context.Stock.Add(stock);
                            _context.Update(Bill);
                            _context.Update(Supplier);

                        }

                    }
                    else
                    {
                        var PurchaseQuote = _context.PurchaseQuote.Include(d => d.Supplier).Where(d => d.PurchaseQuoteID == PurchaseQuoteId).FirstOrDefault();
                        Sanction sanction = new Sanction();

                        sanction.SanctionTitle = "Unvalalid Rapport Test Qualite";
                        sanction.SanctionDescription = "un des normes de qualites de Product est invalid";
                        sanction.SupplierId = PurchaseQuote.SupplierID;
                        _context.Sanction.Add(sanction);





                    }



                }

                if (ServiceQuoteId != 0)
                {


                    QualityTestReport.ServiceQuoteId = ServiceQuoteId;
                    QualityTestReport.QualityTestingDepartmentManagerId = userId;


                    if (QualityTestReport.CntItemsValidity && QualityTestReport.StateValidity && QualityTestReport.OperationValidity)
                    {

                        var ServiceQuote = _context.ServiceQuote.Where(f => f.ServiceQuoteID == ServiceQuoteId).FirstOrDefault();
                        var Bill = _context.BillService.Where(f => f.ServiceQuoteID == ServiceQuoteId).FirstOrDefault();
                        var Supplier = _context.Supplier.Where(f => f.UserID == ServiceQuote.SupplierID).FirstOrDefault();
                        var saleOffer = _context.SaleOffer.Include(so => so.Products).Where(so => so.SupplierId == Supplier.UserID).FirstOrDefault();

                        var stock = new Stock();
                        if (Bill != null && Supplier != null)
                        {
                            Bill.Validity = true;
                            Supplier.TransactionNumber++;
                           


                            _context.Stock.Add(stock);
                            _context.Update(Bill);
                            _context.Update(Supplier);

                        }

                    }
                    else
                    {
                        var ServiceQuote = _context.ServiceQuote.Include(d => d.Supplier).Where(d => d.ServiceQuoteID == ServiceQuoteId).FirstOrDefault();
                        Sanction sanction = new Sanction();

                        sanction.SanctionTitle = "Unvalalid Rapport Test Qualite";
                        sanction.SanctionDescription = "un des normes de qualites de Product est invalid";
                        sanction.SupplierId = ServiceQuote.SupplierID;
                        _context.Sanction.Add(sanction);





                    }



                }




                var notification = _context.NotificationInfo.FirstOrDefault(d => d.UserID == userId);
                if (notification != null)
                {
                    _context.Notification.Remove(notification);
                }


                // Save the QualityTestReport object to the database
                _context.QualityTestReport.Add(QualityTestReport);
                _context.SaveChanges();
            

          
            return View(QualityTestReport);
        }






        // GET: QualityTestReports/Edit/5
        [HttpGet("/QualityTestReports/Edit/{id}")]
        [SwaggerOperation(Summary = "Show quality test report edit form", Description = "Display the quality test report edit form.")]
        [SwaggerResponse(200, "Quality test report edit form displayed successfully.")]
        [SwaggerResponse(404, "Quality test report not found.")]
        public async Task<IActionResult> Edit( int? id)
        {
            if (id == null || _context.QualityTestReport == null)
            {
                return NotFound();
            }

            var QualityTestReport = await _context.QualityTestReport.FindAsync(id);
            if (QualityTestReport == null)
            {
                return NotFound();
            }
            return View(QualityTestReport);
        }






        // POST: QualityTestReports/Edit/5
        [HttpPost("/QualityTestReports/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit a quality test report", Description = "Edit an existing quality test report with the provided information.")]
        [SwaggerResponse(200, "Quality test report edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Quality test report not found.")]
        public async Task<IActionResult> Edit(int id, [Bind("QualityTestReportID,StateValidity,CntItemsValidity,OperationValidity,QualityTestingDepartmentManagerId,PurchaseQuoteId")] QualityTestReport QualityTestReport)
        {
            if (id != QualityTestReport.QualityTestReportID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (QualityTestReport.CntItemsValidity && QualityTestReport.StateValidity && QualityTestReport.OperationValidity)
                    {
                        var PurchaseQuote = _context.PurchaseQuote.Where(f => f.PurchaseQuoteID == QualityTestReport.PurchaseQuoteId).FirstOrDefault();
                        var Bill = _context.BillPurchase.Where(f => f.PurchaseQuoteID == QualityTestReport.PurchaseQuoteId).FirstOrDefault();
                        var Supplier = _context.Supplier.Where(f => f.UserID == PurchaseQuote.SupplierID).FirstOrDefault();
                        var saleOffer = _context.SaleOffer.Include(so => so.Products).Where(so => so.SupplierId == Supplier.UserID).FirstOrDefault();
                        var Products = saleOffer.Products.ToList();

                        if (Bill != null && Supplier != null)
                        {
                            Bill.Validity = true;
                            Supplier.TransactionNumber++;
                            foreach (var product in Products)
                            {
                                var productInStock = _context.Stock
                                    .Where(s => s.Products.Any(p => p.ProductID == product.ProductID))
                                    .FirstOrDefault();

                                if (productInStock == null)
                                {
                                    var stock = new Stock();
                                    stock.Products.Add(product);
                                    _context.Stock.Add(stock);
                                }
                            }

                            _context.Update(Bill);
                            _context.Update(Supplier);
                        }
                    }

                    _context.Update(QualityTestReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QualityTestReportExists(QualityTestReport.QualityTestReportID))
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
            return View(QualityTestReport);
        }





        // GET: QualityTestReports/Delete/5
        [HttpGet("/QualityTestReports/Delete/{id}")]
        [SwaggerOperation(Summary = "Show quality test report delete confirmation", Description = "Display the quality test report delete confirmation.")]
        [SwaggerResponse(200, "Quality test report delete confirmation displayed successfully.")]
        [SwaggerResponse(404, "Quality test report not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.QualityTestReport == null)
            {
                return NotFound();
            }

            var QualityTestReport = await _context.QualityTestReport
                .FirstOrDefaultAsync(m => m.QualityTestReportID == id);
            if (QualityTestReport == null)
            {
                return NotFound();
            }

            return View(QualityTestReport);
        }





        // POST: QualityTestReports/Delete/5
        [HttpPost("/QualityTestReports/DeleteConfirmed/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete a quality test report", Description = "Delete an existing quality test report.")]
        [SwaggerResponse(200, "Quality test report deleted successfully.")]
        [SwaggerResponse(404, "Quality test report not found.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.QualityTestReport == null)
            {
                return Problem("Entity set 'GAPContext.QualityTestReport'  is null.");
            }
            var QualityTestReport = await _context.QualityTestReport.FindAsync(id);
            if (QualityTestReport != null)
            {
                _context.QualityTestReport.Remove(QualityTestReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        /*---------------------------------------------------------------*/





        // Helper: no route
        private bool QualityTestReportExists(int id)
        {
          return (_context.QualityTestReport?.Any(e => e.QualityTestReportID == id)).GetValueOrDefault();
        }
    }
}
