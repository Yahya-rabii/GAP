using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using X.PagedList;

namespace GAP.Controllers
{
    public class ServiceQuotesController : Controller
    {
        private readonly GAPContext _context;

        public ServiceQuotesController(GAPContext context)
        {
            _context = context;
        }

        // GET: ServiceQuotes
        [HttpGet("/ServiceQuotes")]

        public async Task<IActionResult> Index(int? page, string SearchString)
        {

            IQueryable<ServiceQuote> PurchaseQuoteiq = from o in _context.ServiceQuote.Include(o => o.Supplier) select o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                PurchaseQuoteiq = _context.ServiceQuote.Include(o => o.Supplier).Where(o => o.Supplier.Email.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await PurchaseQuoteiq.ToPagedListAsync(pageNumber, pageSize));

        }







        // GET: ServiceQuotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ServiceQuote == null)
            {
                return NotFound();
            }

            var serviceQuote = await _context.ServiceQuote
                .Include(s => s.ServiceOffer)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.ServiceQuoteID == id);
            if (serviceQuote == null)
            {
                return NotFound();
            }

            return View(serviceQuote);
        }








        // GET: PurchaseQuote/Create
        [HttpGet("/ServiceQuotes/Create")]
        [SwaggerOperation(Summary = "Show purchase quote creation form", Description = "Display the purchase quote creation form.")]
        [SwaggerResponse(200, "Purchase quote creation form displayed successfully.")]
        [SwaggerResponse(404, "Sale offer not found.")]
        public IActionResult Create(int ServiceOfferID)
        {
            // Retrieve the SaleOffer object based on the received SaleOfferID

            var ServiceOffer = _context.ServiceOffer.Find(ServiceOfferID);

            if (ServiceOffer == null)
            {
                // Handle the case when the SaleOffer is not found
                return NotFound();
            }

            ServiceOffer.Validity = true;
            _context.Update(ServiceOffer);
            _context.SaveChangesAsync();

            ViewBag.ServiceOfferID = ServiceOfferID;

            return View();
        }






        // POST: PurchaseQuote/Create
        [HttpPost("/ServiceQuotes/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new purchase quote", Description = "Create a new purchase quote for purchasing department managers.")]
        [SwaggerResponse(200, "Purchase quote created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Create(int ServiceOfferID, [Bind("StartDate,EndDate")] ServiceQuote ServiceQuote)
        {
            if (ModelState.IsValid)
            {
                // Get the ID of the currently logged-in user
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var ServiceOffer = await _context.ServiceOffer.FindAsync(ServiceOfferID);
                if (ServiceOffer != null)
                {
                    ServiceQuote.PurchasingDepartmentManagerId = userId;
                    ServiceQuote.ServiceOffer = ServiceOffer;
                    ServiceQuote.ServiceOfferID = ServiceOfferID;
                    ServiceQuote.Price = ServiceOffer.Price ;
                    ServiceQuote.SupplierID = ServiceOffer.SupplierId;



                    var ServiceRequest = await _context.ServiceRequest.FindAsync(ServiceOffer.ServiceRequestId);

                    if (ServiceRequest != null)
                    {
                        ServiceRequest.IsValid = false;
                    }
                    _context.Update(ServiceRequest);

                }




                _context.Add(ServiceQuote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "Email", ServiceQuote.SupplierID);
            return View(ServiceQuote);
        }









        // GET: ServiceQuotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ServiceQuote == null)
            {
                return NotFound();
            }

            var serviceQuote = await _context.ServiceQuote.FindAsync(id);
            if (serviceQuote == null)
            {
                return NotFound();
            }
            ViewData["ServiceOfferID"] = new SelectList(_context.ServiceOffer, "ServiceOfferID", "ServiceOfferID", serviceQuote.ServiceOfferID);
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "UserID", "Discriminator", serviceQuote.SupplierID);
            return View(serviceQuote);
        }

        // POST: ServiceQuotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceQuoteID,StartDate,EndDate,Price,SupplierID,PurchasingDepartmentManagerId,ServiceOfferID")] ServiceQuote serviceQuote)
        {
            if (id != serviceQuote.ServiceQuoteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceQuote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceQuoteExists(serviceQuote.ServiceQuoteID))
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
            ViewData["ServiceOfferID"] = new SelectList(_context.ServiceOffer, "ServiceOfferID", "ServiceOfferID", serviceQuote.ServiceOfferID);
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "UserID", "Discriminator", serviceQuote.SupplierID);
            return View(serviceQuote);
        }

        // GET: ServiceQuotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ServiceQuote == null)
            {
                return NotFound();
            }

            var serviceQuote = await _context.ServiceQuote
                .Include(s => s.ServiceOffer)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.ServiceQuoteID == id);
            if (serviceQuote == null)
            {
                return NotFound();
            }

            return View(serviceQuote);
        }

        // POST: ServiceQuotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ServiceQuote == null)
            {
                return Problem("Entity set 'GAPContext.ServiceQuote'  is null.");
            }
            var serviceQuote = await _context.ServiceQuote.FindAsync(id);
            if (serviceQuote != null)
            {
                _context.ServiceQuote.Remove(serviceQuote);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceQuoteExists(int id)
        {
          return (_context.ServiceQuote?.Any(e => e.ServiceQuoteID == id)).GetValueOrDefault();
        }
    }
}
