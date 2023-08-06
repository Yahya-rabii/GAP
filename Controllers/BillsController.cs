using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;
using Xceed.Document.NET;
using Xceed.Words.NET;
using X.PagedList;

namespace GAP.Controllers
{

    [Authorize]
    [Authorize(Roles = "FinanceDepartmentManager")]
    public class BillsController : Controller
    {
        private readonly GAPContext _context;

        public BillsController(GAPContext context)
        {
            _context = context;
        }

        // GET: Bills
        public async Task<IActionResult> Index(int? page)
        {
            IQueryable<Bill> iseriq = from f in _context.Bill
                                      select f;


            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        
           
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bill == null)
            {
                return NotFound();
            }

            var Bill = await _context.Bill
                .FirstOrDefaultAsync(m => m.BillID == id);

            var PurchaseQuote = await _context.PurchaseQuote.Include(d=>d.Products).Include(d => d.Supplier)
                .FirstOrDefaultAsync(m => m.PurchaseQuoteID == Bill.PurchaseQuoteID);

            if (Bill == null || PurchaseQuote == null)
            {
                return NotFound();
            }

            // Load the existing Word document template
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "Bill.docx");
            using (DocX document = DocX.Load(templatePath))
            {
                // Replace placeholders in the template with the corresponding data
                document.ReplaceText("Numéro de Bill :", "Numéro de Bill : #" + DateTime.Now.Date.ToString("yyyyMMdd") + "-" +Bill.BillID.ToString());
                document.ReplaceText("Date de Création :", "Date de Création : " + PurchaseQuote.CreationDate.Date.ToString());
                document.ReplaceText("Name du Supplier :", "Name du Supplier : " + PurchaseQuote.Supplier.Name.ToString());
                document.ReplaceText("Email Supplier :", "Email Supplier : " + PurchaseQuote.Supplier.Email.ToString());
                document.ReplaceText("Adresse Supplier :", "Adresse Supplier : " + PurchaseQuote.Supplier.Adresse.ToString());
                document.ReplaceText("Code Postal Supplier :", "Code Postal Supplier : " + PurchaseQuote.Supplier.PostalCode.ToString());
                document.ReplaceText("Numéro de téléphone Supplier :", "Numéro de téléphone Supplier : " + PurchaseQuote.Supplier.PhoneNumber.ToString());

                // Check if the document has at least one table
                if (document.Tables.Count > 2)
                {
                    // Get the third table in the document
                    Table table = document.Tables[2];

                    float MTotalHT=0;
                    float TotalTVA=0;
                    float TotalTTC=0;


                    // Iterate through each item in PurchaseQuote.Products and add a row for each item in the table
                    foreach (var Product in PurchaseQuote.Products)
                    {
                        Row row = table.InsertRow();
                        row.Cells[0].Paragraphs[0].Append(Product.Desc);
                        row.Cells[1].Paragraphs[0].Append(Product.ItemsNumber.ToString());
                        row.Cells[2].Paragraphs[0].Append(Product.Unitprice.ToString());
                        row.Cells[3].Paragraphs[0].Append("20%");
                        row.Cells[4].Paragraphs[0].Append(Product.Totalprice.ToString());

                        MTotalHT += Product.Totalprice;
                        TotalTVA += Product.Totalprice * 20 / 100;

                    }

                    // You can add more rows and populate them with data as needed.

                    var offre = _context.SaleOffer.Where(o => o.SupplierId == PurchaseQuote.SupplierID).FirstOrDefault();

                    TotalTTC = (float)(MTotalHT + TotalTVA + offre.TotalProfit);
                    document.ReplaceText("Profit Supplier :", "Profit Supplier : " + offre.TotalProfit.ToString());
                    document.ReplaceText("Montant Total HT :", "Montant Total HT : " + MTotalHT.ToString());
                    document.ReplaceText("Total TVA :", "Total TVA : " + TotalTVA.ToString());
                    document.ReplaceText("Montant Total TTC :", "Montant Total TTC : " + TotalTTC.ToString()); // Corrected line
                    document.ReplaceText("Montant déjà versé :", "Montant déjà versé : 0"); ;
                    document.ReplaceText("Reste à payer :", "Reste à payer : " + TotalTTC.ToString());

                






                }

                // Save the updated document with the new data
                string dateFormatted = DateTime.Now.Date.ToString("yyyyMMdd");
                string fileName = $"fact-{PurchaseQuote.Supplier.Name}-{dateFormatted}.docx";
                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", fileName);
                document.SaveAs(outputPath);
            }

            // Return the file for download or other processing (e.g., you can use a FileResult)
            return View(Bill);
        }

        // GET: Bills/Create
        public IActionResult Create(int PurchaseQuoteId)
        {
            // Store the PurchaseRequestId in ViewBag or ViewData so that it can be used in the view.
            ViewBag.PurchaseQuoteId = PurchaseQuoteId;

 


       
            return View();
        }

        // POST: Bills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int PurchaseQuoteId, [Bind("BillID")] Bill Bill)
        {

            if (PurchaseQuoteId == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)

            {
                
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var PurchaseQuote = _context.PurchaseQuote.Include(d=>d.Supplier).FirstOrDefault(d => d.PurchaseQuoteID == PurchaseQuoteId) ;


                Bill.PurchaseQuoteID = PurchaseQuoteId;
                Bill.FinanceDepartmentManagerId = userId;
                Bill.Validity = false;




                var notification = _context.NotificationInfo.FirstOrDefault(d => d.UserID == userId);
                if (notification != null)
                {
                    _context.Notification.Remove(notification);
                }


                _context.Add(Bill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Bill);
        }

        // GET: Bills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bill == null)
            {
                return NotFound();
            }

            var Bill = await _context.Bill.FindAsync(id);
            if (Bill == null)
            {
                return NotFound();
            }
            return View(Bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillID,Prix,SupplierEmail,Validity,FinanceDepartmentManagerId,PurchaseQuoteID")] Bill Bill)
        {
            if (id != Bill.BillID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Bill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillExists(Bill.BillID))
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
            return View(Bill);
        }

        // GET: Bills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bill == null)
            {
                return NotFound();
            }

            var Bill = await _context.Bill
                .FirstOrDefaultAsync(m => m.BillID == id);
            if (Bill == null)
            {
                return NotFound();
            }

            return View(Bill);
        }

        // POST: Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bill == null)
            {
                return Problem("Entity set 'GAPContext.Bill'  is null.");
            }
            var Bill = await _context.Bill.FindAsync(id);
            if (Bill != null)
            {
                _context.Bill.Remove(Bill);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillExists(int id)
        {
          return (_context.Bill?.Any(e => e.BillID == id)).GetValueOrDefault();
        }
    }
}
