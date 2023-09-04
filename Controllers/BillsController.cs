using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;
using Xceed.Document.NET;
using Xceed.Words.NET;
using X.PagedList;
using iTextSharp.text.pdf;
using Swashbuckle.AspNetCore.Annotations;

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
        [HttpGet("/Bills")]
        [SwaggerOperation(Summary = "View list of bills", Description = "Display a list of bills.")]
        [SwaggerResponse(200, "List of bills displayed successfully.")]
        public async Task<IActionResult> Index(int? page)
        {
            IQueryable<Bill> iseriq = from f in _context.Bill
                                      select f;


            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        
           
        }





        // GET: Bills/Details
        [HttpGet("/Bills/Details/{id}")]
        [SwaggerOperation(Summary = "View bill details", Description = "Display details of a bill.")]
        [SwaggerResponse(200, "Bill details displayed successfully.")]
        [SwaggerResponse(404, "Bill not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bill == null)
            {
                return NotFound();
            }

            var Bill = await _context.BillPurchase
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
                document.ReplaceText("Numéro de facture :", "Numéro de facture : #" + DateTime.Now.Date.ToString("yyyyMMdd") + "-" +Bill.BillID.ToString());
                document.ReplaceText("Date de Création :", "Date de Création : " + PurchaseQuote.CreationDate.Date.ToString());
                document.ReplaceText("Nom du Fournisseur :", "Nom du Fournisseur : " + PurchaseQuote.Supplier.CompanyName.ToString());
                document.ReplaceText("Email Fournisseur :", "Email Fournisseur : " + PurchaseQuote.Supplier.Email.ToString());
                document.ReplaceText("Adresse Fournisseur :", "Adresse Fournisseur : " + PurchaseQuote.Supplier.Adresse.ToString());
                document.ReplaceText("Code Postal Fournisseur :", "Code Postal Fournisseur : " + PurchaseQuote.Supplier.PostalCode.ToString());
                document.ReplaceText("Numéro de téléphone Fournisseur :", "Numéro de téléphone Fournisseur : " + PurchaseQuote.Supplier.PhoneNumber.ToString());

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
                    document.ReplaceText("Profit Fournisseur :", "Profit Fournisseur : " + offre.TotalProfit.ToString());
                    document.ReplaceText("Montant Total HT :", "Montant Total HT : " + MTotalHT.ToString());
                    document.ReplaceText("Total TVA :", "Total TVA : " + TotalTVA.ToString());
                    document.ReplaceText("Montant Total TTC :", "Montant Total TTC : " + TotalTTC.ToString()); // Corrected line
                    document.ReplaceText("Montant déjà versé :", "Montant déjà versé : 0"); ;
                    document.ReplaceText("Reste à payer :", "Reste à payer : " + TotalTTC.ToString());

              

                }

                // Save the updated document with the new data
                string dateFormatted = DateTime.Now.Date.ToString("yyyyMMdd");
                string fileName = $"fact-{PurchaseQuote.Supplier.CompanyName}-{dateFormatted}.docx";
                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", fileName);
                document.SaveAs(outputPath);

                // Pass the fileName to the view
                ViewData["PdfFileName"] = fileName;
            }

            // Return the file for download or other processing (e.g., you can use a FileResult)
            return View(Bill);
        }




        [HttpGet("/Bills/ViewPdf/{filename}")]
        [SwaggerOperation(Summary = "View PDF file", Description = "Display a PDF file.")]
        [SwaggerResponse(200, "PDF file displayed successfully.")]
        public IActionResult ViewPdf(string filename)
        {
            // Load the DOCX document
            string docxPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", filename);
            Spire.Doc.Document document = new Spire.Doc.Document();
            document.LoadFromFile(docxPath);

            // Create a PDF stream
            MemoryStream pdfStream = new MemoryStream();

            // Save the document as PDF
            document.SaveToStream(pdfStream, Spire.Doc.FileFormat.PDF);

            // Reset the PDF stream position
            pdfStream.Position = 0;

            // Load the PDF into iTextSharp
            PdfReader reader = new PdfReader(pdfStream.ToArray());

            using (MemoryStream outputStream = new MemoryStream())
            {
                using (PdfStamper stamper = new PdfStamper(reader, outputStream))
                {
                    // Remove any existing watermarks and text
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfContentByte content = stamper.GetUnderContent(i);
                        // Clearing content on the page (you might need to adjust coordinates)
                        content.Reset();
                    }
                }
                string filenameWithoutExtension = filename.Replace(".docx", "");
                string output = filenameWithoutExtension + ".pdf";

                // Return the PDF for viewing
                return File(outputStream.ToArray(), "application/pdf",output);
            }
        }






        // GET: Bills/Create
        [HttpGet("/Bills/Create")]
        [SwaggerOperation(Summary = "Create a new bill", Description = "Display the form to create a new bill.")]
        [SwaggerResponse(200, "Form to create a new bill displayed successfully.")]
        public IActionResult Create(int PurchaseQuoteId, int ServiceQuoteId)
        {
            // Store the PurchaseRequestId in ViewBag or ViewData so that it can be used in the view.
            ViewBag.PurchaseQuoteId = PurchaseQuoteId;
            ViewBag.ServiceQuoteId = ServiceQuoteId;

 


       
            return View();
        }





        // POST: Bills/Create
        [HttpPost("/Bills/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new bill", Description = "Handle the creation of a new bill.")]
        [SwaggerResponse(200, "Bill created successfully.")]
        public async Task<IActionResult> Create(int PurchaseQuoteId, int ServiceQuoteId, [Bind("BillID")] Bill Bill)
        {

            if (PurchaseQuoteId == 0 && ServiceQuoteId == 0)
            {
                return NotFound();
            }

            else { 

      
                if(PurchaseQuoteId != 0) {
                   var BillPurchase = new BillPurchase();
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var PurchaseQuote = _context.PurchaseQuote.Include(d=>d.Supplier).FirstOrDefault(d => d.PurchaseQuoteID == PurchaseQuoteId) ;

                    BillPurchase.BillID = Bill.BillID;
                    BillPurchase.PurchaseQuoteID = PurchaseQuoteId;
                    BillPurchase.FinanceDepartmentManagerId = userId;
                    BillPurchase.Validity = false;




                var notification = _context.NotificationInfo.FirstOrDefault(d => d.UserID == userId);
                if (notification != null)
                {
                    _context.Notification.Remove(notification);
                }


                _context.Add(BillPurchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

                } 
                
                if(ServiceQuoteId != 0) { 
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var ServiceQuote = _context.ServiceQuote.Include(d=>d.Supplier).FirstOrDefault(d => d.ServiceQuoteID == ServiceQuoteId) ;
                    var BillService = new BillService();

                    BillService.BillID = Bill.BillID;
                    BillService.ServiceQuoteID = ServiceQuoteId;
                    BillService.FinanceDepartmentManagerId = userId;
                    BillService.Validity = false;




                var notification = _context.NotificationInfo.FirstOrDefault(d => d.UserID == userId);
                if (notification != null)
                {
                    _context.Notification.Remove(notification);
                }


                _context.Add(BillService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

                }


                return View();

            }
        }






        // GET: Bills/Edit/5
        [HttpGet("/Bills/Edit/{id}")]
        [SwaggerOperation(Summary = "Edit a bill", Description = "Display the form to edit a bill.")]
        [SwaggerResponse(200, "Form to edit a bill displayed successfully.")]
        [SwaggerResponse(404, "Bill not found.")]
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
        [HttpPost("/Bills/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit a bill", Description = "Handle the editing of a bill.")]
        [SwaggerResponse(200, "Bill edited successfully.")]
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
        [HttpGet("/Bills/Delete/{id}")]
        [SwaggerOperation(Summary = "Delete a bill", Description = "Display the confirmation form to delete a bill.")]
        [SwaggerResponse(200, "Confirmation form to delete a bill displayed successfully.")]
        [SwaggerResponse(404, "Bill not found.")]
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
        [HttpPost("/Bills/Delete/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete a bill", Description = "Handle the deletion of a bill.")]
        [SwaggerResponse(200, "Bill deleted successfully.")]
        [SwaggerResponse(400, "Entity set 'GAPContext.Bill' is null.")]
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
