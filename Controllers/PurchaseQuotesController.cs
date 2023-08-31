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
using X.PagedList;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Controllers
{
    [Authorize]
    [Authorize(Roles = "PurchasingDepartmentManager")]
    public class PurchaseQuotesController : Controller
    {
        private readonly GAPContext _context;

        public PurchaseQuotesController(GAPContext context)
        {
            _context = context;
        }




        // GET: PurchaseQuote
        [HttpGet("/PurchaseQuotes")]
        [SwaggerOperation(Summary = "Get purchase quotes", Description = "Retrieve a list of purchase quotes for purchasing department managers.")]
        [SwaggerResponse(200, "List of purchase quotes for purchasing department managers retrieved successfully.")]
        public async Task<IActionResult> Index(int? page, string SearchString)
        {


            IQueryable<PurchaseQuote> PurchaseQuoteiq = from o in _context.PurchaseQuote.Include(o => o.Supplier) select o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                PurchaseQuoteiq = _context.PurchaseQuote.Include(o => o.Supplier).Where(o => o.Supplier.Email.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await PurchaseQuoteiq.ToPagedListAsync(pageNumber, pageSize));


        }





        // GET: PurchaseQuote/Details/5
        [HttpGet("/PurchaseQuotes/Details/{id}")]
        [SwaggerOperation(Summary = "Get details of a purchase quote", Description = "Retrieve the details of a purchase quote for purchasing department managers.")]
        [SwaggerResponse(200, "Purchase quote details retrieved successfully.")]
        [SwaggerResponse(404, "Purchase quote not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PurchaseQuote == null)
            {
                return NotFound();
            }

            var PurchaseQuote = await _context.PurchaseQuote
                .Include(d => d.Supplier)
                .Include(d=>d.SaleOffer)
                .ThenInclude(o=>o.PurchaseRequest)
                .Include(d=>d.Products)
                .FirstOrDefaultAsync(m => m.PurchaseQuoteID == id);

            if (PurchaseQuote == null)
            {
                return NotFound();
            }

          

            return View(PurchaseQuote);
        }







        // GET: PurchaseQuote/Create
        [HttpGet("/PurchaseQuotes/Create")]
        [SwaggerOperation(Summary = "Show purchase quote creation form", Description = "Display the purchase quote creation form.")]
        [SwaggerResponse(200, "Purchase quote creation form displayed successfully.")]
        [SwaggerResponse(404, "Sale offer not found.")]
        public IActionResult Create(int SaleOfferID)
        {
            // Retrieve the SaleOffer object based on the received SaleOfferID
            
            var SaleOffer = _context.SaleOffer.Find(SaleOfferID);

            if (SaleOffer == null)
            {
                // Handle the case when the SaleOffer is not found
                return NotFound();
            }           
            
            SaleOffer.Validity = true;
            _context.Update(SaleOffer);
            _context.SaveChangesAsync();

            ViewBag.SaleOfferID = SaleOfferID;

            return View();
        }






        // POST: PurchaseQuote/Create
        [HttpPost("/PurchaseQuotes/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new purchase quote", Description = "Create a new purchase quote for purchasing department managers.")]
        [SwaggerResponse(200, "Purchase quote created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Create(int SaleOfferID, [Bind("PurchaseQuoteID,ReceptionDate")] PurchaseQuote PurchaseQuote)
        {
            if (ModelState.IsValid)
            {
                // Get the ID of the currently logged-in user
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var SaleOffer = await _context.SaleOffer.FindAsync(SaleOfferID);
                if (SaleOffer != null)
                {
                    PurchaseQuote.PurchasingDepartmentManagerId = userId;
                    PurchaseQuote.SaleOffer = SaleOffer;
                    PurchaseQuote.SaleOfferID = SaleOfferID;
                    PurchaseQuote.CreationDate = DateTime.Now;

                    // Use ExecuteSqlRawAsync with a parameter to fetch related Product entities
                    var list = await _context.Product
                        .FromSqlRaw("SELECT * FROM Product WHERE SaleOfferId = {0}", SaleOfferID)
                        .ToListAsync();

                    PurchaseQuote.Products = list;
                    float ?prixTTLpiece=0;
                    int ?Nbrpiece=0;

                    foreach(var p in SaleOffer.Products)
                    {
                        prixTTLpiece += p.ItemsNumber * p.Unitprice;
                        Nbrpiece += p.ItemsNumber;
                    } 
                    
                    PurchaseQuote.TotalPrice = prixTTLpiece +   SaleOffer.UnitProfit * Nbrpiece;
                    PurchaseQuote.typeCntProducts = SaleOffer.Products.Count();
                    PurchaseQuote.SupplierID = SaleOffer.SupplierId;



                    var  PurchaseRequest = await _context.PurchaseRequest.FindAsync(SaleOffer.PurchaseRequestId);

                    if (PurchaseRequest != null)
                    {
                        PurchaseRequest.IsValid = false;
                    }
                    _context.Update(PurchaseRequest);

                }


                

                _context.Add(PurchaseQuote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "Email", PurchaseQuote.SupplierID);
            return View(PurchaseQuote);
        }






        [HttpGet("/PurchaseQuotes/Edit/{id}")]
        [SwaggerOperation(Summary = "Show purchase quote edit form", Description = "Display the purchase quote edit form for purchasing department managers.")]
        [SwaggerResponse(200, "Purchase quote edit form displayed successfully.")]
        [SwaggerResponse(404, "Purchase quote not found.")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PurchaseQuote == null)
            {
                return NotFound();
            }

            var PurchaseQuote = await _context.PurchaseQuote.FindAsync(id);
            if (PurchaseQuote == null)
            {
                return NotFound();
            }
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "Email", PurchaseQuote.SupplierID);
            return View(PurchaseQuote);
        }






        // POST: PurchaseQuote/Edit/5
        [HttpPost("/PurchaseQuotes/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit a purchase quote", Description = "Edit an existing purchase quote for purchasing department managers.")]
        [SwaggerResponse(200, "Purchase quote edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Purchase quote not found.")]
        public async Task<IActionResult> Edit(int id, [Bind("PurchaseQuoteID,CreationDate,ReceptionDate,ProductID,UnitProfit,typeCntProducts,SupplierID,PurchasingDepartmentManagerId")] PurchaseQuote PurchaseQuote)
        {
            if (id != PurchaseQuote.PurchaseQuoteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(PurchaseQuote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseQuoteExists(PurchaseQuote.PurchaseQuoteID))
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
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "Email", PurchaseQuote.SupplierID);
            return View(PurchaseQuote);
        }






        // GET: PurchaseQuote/Delete/5
        [HttpGet("/PurchaseQuotes/Delete/{id}")]
        [SwaggerOperation(Summary = "Show purchase quote delete confirmation", Description = "Display the purchase quote delete confirmation for purchasing department managers.")]
        [SwaggerResponse(200, "Purchase quote delete confirmation displayed successfully.")]
        [SwaggerResponse(404, "Purchase quote not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PurchaseQuote == null)
            {
                return NotFound();
            }

            var PurchaseQuote = await _context.PurchaseQuote
                .Include(d => d.Supplier)
                .FirstOrDefaultAsync(m => m.PurchaseQuoteID == id);
            if (PurchaseQuote == null)
            {
                return NotFound();
            }

            return View(PurchaseQuote);
        }







        // POST: PurchaseQuote/Delete/5
        [HttpPost("/PurchaseQuotes/DeleteConfirmed/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete a purchase quote", Description = "Delete an existing purchase quote for purchasing department managers.")]
        [SwaggerResponse(200, "Purchase quote deleted successfully.")]
        [SwaggerResponse(404, "Purchase quote not found.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PurchaseQuote == null)
            {
                return Problem("Entity set 'GAPContext.PurchaseQuote'  is null.");
            }
            var PurchaseQuote = await _context.PurchaseQuote.FindAsync(id);
            if (PurchaseQuote != null)
            {
                _context.PurchaseQuote.Remove(PurchaseQuote);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }






        /*---------------------------------------------------------------*/






        // Helper: no route
        private bool PurchaseQuoteExists(int id)
        {
          return (_context.PurchaseQuote?.Any(e => e.PurchaseQuoteID == id)).GetValueOrDefault();
        }
    }
}
