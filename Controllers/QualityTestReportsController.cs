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

namespace GAP.Controllers
{
    public class QualityTestReportsController : Controller
    {
        private readonly GAPContext _context;

        public QualityTestReportsController(GAPContext context)
        {
            _context = context;
        }
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

            int pageSize = 2;
            int pageNumber = (page ?? 1);

            return View(await QualityTestReports.ToPagedListAsync(pageNumber, pageSize));
        }





        // GET: QualityTestReports/Details/5
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
        public IActionResult Create(int PurchaseQuoteId)
        {
            // Fetch the list of all PurchaseQuote with corresponding product and supplier names
            var PurchaseQuoteList = _context.PurchaseQuote.Where(d => d.PurchaseQuoteID == PurchaseQuoteId)
               .Include(d => d.Supplier)
               .Select(d => new SelectListItem
               {
                   Value = d.PurchaseQuoteID.ToString(),
                   Text = $"PurchaseQuote: {d.PurchaseQuoteID} | provider id : {d.Supplier.Email}" // Combine product name and supplier email
               })
               .ToList();

            // Create the select list for dropdown menu
            ViewBag.PurchaseQuoteList = new SelectList(PurchaseQuoteList, "Value", "Text");
            ViewBag.PurchaseQuoteId = PurchaseQuoteId;

            return View();
        }

        // POST: QualityTestReport/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int PurchaseQuoteId,  QualityTestReport QualityTestReport)
        {
            if (ModelState.IsValid)
            {

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                QualityTestReport.PurchaseQuoteId = PurchaseQuoteId;
                QualityTestReport.QualityTestingDepartmentManagerId = userId;


                if (QualityTestReport.CntItemsValidity && QualityTestReport.StateValidity && QualityTestReport.OperationValidity)
                {

                    var PurchaseQuote = _context.PurchaseQuote.Where(f=>f.PurchaseQuoteID ==  PurchaseQuoteId).FirstOrDefault();
                    var Bill  = _context.Bill.Where(f=>f.PurchaseQuoteID ==  PurchaseQuoteId).FirstOrDefault();
                    var Supplier  = _context.Supplier.Where(f=>f.SupplierID==PurchaseQuote.SupplierID).FirstOrDefault();
                    var saleOffer = _context.SaleOffer.Include(so=>so.Products).Where(so=>so.SupplierId==Supplier.SupplierID).FirstOrDefault();   
                    var Products = saleOffer.Products.ToList();

                    var stock = new Stock();
                    if (Bill != null && Supplier !=null)
                    {
                        Bill.Validity =  true;
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
                    var PurchaseQuote = _context.PurchaseQuote.Include(d=>d.Supplier).Where(d=>d.PurchaseQuoteID==PurchaseQuoteId).FirstOrDefault();
                    Sanction sanction = new Sanction();

                    sanction.SanctionTitle = "Unvalalid Rapport Test Qualite";
                    sanction.SanctionDescription = "un des normes de qualites de Product est invalid";
                    sanction.SupplierId = PurchaseQuote.SupplierID;
                    _context.Sanction.Add(sanction);





                }



                var notification = _context.NotificationInfo.FirstOrDefault(d => d.UserID == userId);
                if (notification != null)
                {
                    _context.Notification.Remove(notification);
                }


                // Save the QualityTestReport object to the database
                _context.QualityTestReport.Add(QualityTestReport);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, refill the dropdown list and return the view with validation errors.
            var PurchaseQuoteList = _context.PurchaseQuote
                .Include(d => d.Supplier)
                .Select(d => new
                {
                    PurchaseQuoteID = d.PurchaseQuoteID,
                    PurchaseQuoteSupplierName = d.Supplier.Email
                })
                .ToList();

            // Create the select list for dropdown menu
            ViewBag.PurchaseQuoteList = new SelectList(PurchaseQuoteList, "PurchaseQuoteID", "PurchaseQuoteSupplierName");

            return View(QualityTestReport);
        }

        // GET: QualityTestReports/Edit/5
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                        var Bill = _context.Bill.Where(f => f.PurchaseQuoteID == QualityTestReport.PurchaseQuoteId).FirstOrDefault();
                        var Supplier = _context.Supplier.Where(f => f.SupplierID == PurchaseQuote.SupplierID).FirstOrDefault();
                        var saleOffer = _context.SaleOffer.Include(so => so.Products).Where(so => so.SupplierId == Supplier.SupplierID).FirstOrDefault();
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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

        private bool QualityTestReportExists(int id)
        {
          return (_context.QualityTestReport?.Any(e => e.QualityTestReportID == id)).GetValueOrDefault();
        }
    }
}
