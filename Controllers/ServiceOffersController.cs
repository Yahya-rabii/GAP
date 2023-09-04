using GAP.Data;
using GAP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;
using System.Linq;
using System.Security.Claims;
using X.PagedList;

namespace GAP.Controllers
{
    public class ServiceOffersController : Controller
    {
        private readonly GAPContext _context;

        public ServiceOffersController(GAPContext context)
        {
            _context = context;
        }


        // GET: ServiceOffers
        [HttpGet("/ServiceOffers")]
        [Authorize(Roles = "Supplier")]
        [SwaggerOperation(Summary = "Get a list of sale offers", Description = "Retrieve a list of sale offers.")]
        [SwaggerResponse(200, "List of sale offers retrieved successfully.")]
        public async Task<IActionResult> Index(int? page, string SearchString)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            IQueryable<ServiceOffer> ServiceOfferiq = from o in _context.ServiceOffer.Include(o => o.Supplier).Include(o=>o.ServiceRequest).Where(o => o.SupplierId == userId) select o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                ServiceOfferiq = ServiceOfferiq
                    .Include(o => o.Supplier)
                    .Where(o => o.Supplier.CompanyName.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var pagedServiceOffers = await ServiceOfferiq.ToPagedListAsync(pageNumber, pageSize);

            // Step 1: Get ServiceOffer IDs from ServiceQuote
            var ServiceOfferIdsfromServiceQuote = await _context.ServiceQuote
                .Select(ServiceQuote => ServiceQuote.ServiceOfferID)
                .ToListAsync();

            // Step 2: Get offre de vente with ServiceQuote
            var ServiceOfferWithServiceQuote = await _context.ServiceOffer
                .Where(owd => ServiceOfferIdsfromServiceQuote.Contains(owd.ServiceOfferID))
                .ToListAsync();

            // Step 3: Get ServiceOffer IDs without ServiceQuote
            var ServiceOffersWithoutServiceQuote = await _context.ServiceOffer
                .Where(owtd => !ServiceOfferIdsfromServiceQuote.Contains(owtd.ServiceOfferID))
                .ToListAsync();

            // Step 4: Check if ServiceOffer without ServiceQuote that I have already got have the same ServiceRequestid as ServiceOffer with ServiceQuote I have already got. If so, delete ServiceOffer without ServiceQuote
            foreach (var offreWithoutServiceQuote in ServiceOffersWithoutServiceQuote)
            {
                foreach (var offreWithServiceQuote in ServiceOfferWithServiceQuote)
                {
                    if (offreWithoutServiceQuote.ServiceRequestId == offreWithServiceQuote.ServiceRequestId)
                    {
                        NotificationSupplier notificationSupplier = new NotificationSupplier();
                        notificationSupplier.NotificationTitle = "offre de vente refuse";
                        notificationSupplier.SupplierID = offreWithoutServiceQuote.SupplierId;
                        notificationSupplier.ServiceOfferID = offreWithoutServiceQuote.ServiceOfferID;

                        _context.NotificationSupplier.Add(notificationSupplier);

                        // Update products associated with this ServiceOffer without ServiceQuote to set ServiceOfferId to null using ExecuteSqlRawAsync
                        // var ServiceOfferIdParam = new SqlParameter("@ServiceOfferId", offreWithoutServiceQuote.ServiceOfferID);
                        //await _context.Database.ExecuteSqlRawAsync("UPDATE Product SET ServiceOfferId = NULL WHERE ServiceOfferId = @ServiceOfferId", ServiceOfferIdParam);

                        // Remove the ServiceOffer without ServiceQuote
                        // _context.ServiceOffer.Remove(offreWithoutServiceQuote);
                        break; // Exit the inner loop once a match is found and deletion is performed
                    }
                }
            }

            await _context.SaveChangesAsync(); // Save the changes to the database

            return View(pagedServiceOffers);





        }






        // GET: ServiceOffers
        [HttpGet("/ServiceOffers/IndexRespSA")]
        [SwaggerOperation(Summary = "Get sale offers for purchasing department manager", Description = "Retrieve sale offers for the purchasing department manager.")]
        [SwaggerResponse(200, "Sale offers retrieved successfully.")]
        [Authorize(Roles = "PurchasingDepartmentManager")]
        public async Task<IActionResult> IndexRespSA(int? page, string SearchString)
        {


            IQueryable<ServiceOffer> ServiceOfferiq = from o in _context.ServiceOffer.Include(o => o.Supplier).Include(o => o.ServiceRequest) select o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                ServiceOfferiq = _context.ServiceOffer.Include(o => o.ServiceRequest).Where(o => o.ServiceRequest.Description.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await ServiceOfferiq.ToPagedListAsync(pageNumber, pageSize));



        }




        // GET: ServiceOffers/Details/5
        [HttpGet("/ServiceOffers/Details/{id}")]
        [SwaggerOperation(Summary = "Get sale offer details", Description = "Retrieve details of a sale offer.")]
        [SwaggerResponse(200, "Sale offer details retrieved successfully.")]
        [SwaggerResponse(404, "Sale offer not found.")]
        [Authorize(Roles = "Supplier,PurchasingDepartmentManager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ServiceOffer == null)
            {
                return NotFound();
            }

            var ServiceOffer = await _context.ServiceOffer
                .Include(o => o.Supplier)
                .Include(o => o.ServiceRequest) // Include the ServiceRequest
                .FirstOrDefaultAsync(m => m.ServiceOfferID == id);

            if (ServiceOffer == null)
            {
                return NotFound();
            }

            return View(ServiceOffer);
        }






        // GET: ServiceOffers/Create
        [Authorize(Roles = "Supplier")]
        [HttpGet("/ServiceOffers/Create")]
        [SwaggerOperation(Summary = "Show sale offer creation form", Description = "Display the sale offer creation form.")]
        [SwaggerResponse(200, "Sale offer creation form displayed successfully.")]
        public async Task<IActionResult> Create(int ServiceRequestId)
        {
            // Store the ServiceRequestId in ViewBag so that it can be used in the view.
            ViewBag.ServiceRequestId = ServiceRequestId;

  
            return View();
        }





        // POST: ServiceOffers/Create
        [HttpPost("/ServiceOffers/Create")]
        [Authorize(Roles = "Supplier")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new sale offer", Description = "Create a new sale offer with the provided information.")]
        [SwaggerResponse(200, "Sale offer created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Create(int ServiceRequestId, ServiceOffer ServiceOffer, int selectedProductId)
        {
            if (ServiceRequestId == 0)
            {
                return NotFound();
            }

            var ServiceRequest = _context.ServiceRequest.Find(ServiceRequestId);

            if (ServiceRequest == null)
            {
                return NotFound();
            }


            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _context.Supplier.FirstOrDefault(u => u.UserID == userId);
            ServiceOffer.SupplierId = userId;
            ServiceOffer.Supplier = user;
            ServiceOffer.ServiceRequestId = ServiceRequestId;
            ServiceOffer.ServiceRequest = ServiceRequest;


           
            _context.Add(ServiceOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }






        // POST: ServiceOffers/ValidateOffre
        [Authorize(Roles = "PurchasingDepartmentManager")]
        [HttpPost("/ServiceOffers/ValidateOffre/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Validate sale offer", Description = "Validate a sale offer.")]
        [SwaggerResponse(200, "Sale offer validated successfully.")]
        [SwaggerResponse(404, "Sale offer not found.")]
        public async Task<IActionResult> ValidateOffre(int? id)
        {
            if (id == null || _context.ServiceOffer == null)
            {
                return NotFound();
            }

            var ServiceOffer = await _context.ServiceOffer.FindAsync(id);
            if (ServiceOffer == null)
            {
                return NotFound();
            }
            else
            {
                // Update the Validity property to false
                ServiceOffer.Validity = true;

                // Mark the entity as modified and save changes
                _context.Update(ServiceOffer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("IndexRespSA", "ServiceOffers");
        }













        [Authorize(Roles = "Supplier")]
        [HttpGet("/ServiceOffers/Edit/{id}")]
        [SwaggerOperation(Summary = "Show sale offer editing form", Description = "Display the form for editing a sale offer's information.")]
        [SwaggerResponse(200, "Sale offer editing form displayed successfully.")]
        [SwaggerResponse(404, "Sale offer not found.")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ServiceOffer = await _context.ServiceOffer
                .FirstOrDefaultAsync(o => o.ServiceOfferID == id);

            if (ServiceOffer == null)
            {
                return NotFound();
            }

            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierID", "Email", ServiceOffer.SupplierId);





            return View(ServiceOffer);
        }










        [HttpPost("/ServiceOffers/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supplier")]
        [SwaggerOperation(Summary = "Edit sale offer information", Description = "Edit the information of a sale offer.")]
        [SwaggerResponse(200, "Sale offer information edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Sale offer not found.")]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceOfferID, UnitProfit, SelectedProductId")] ServiceOffer updatedServiceOffer)
        {
            // Remove the ModelState check, as it might be causing issues

            // Fetch the existing "ServiceOffer" record from the database
            var existingServiceOffer = await _context.ServiceOffer
                .FirstOrDefaultAsync(o => o.ServiceOfferID == id);

            if (existingServiceOffer == null)
            {
                return NotFound();
            }



            try
            {
                _context.Update(existingServiceOffer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceOfferExists(existingServiceOffer.ServiceOfferID))
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








        // GET: ServiceOffers/Delete/5
        [HttpGet("/ServiceOffers/Delete/{id}")]
        [SwaggerOperation(Summary = "Show sale offer deleting form", Description = "Display the form for deleting a sale offer's information.")]
        [SwaggerResponse(200, "Sale offer deleting form displayed successfully.")]
        [SwaggerResponse(404, "Sale offer not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ServiceOffer == null)
            {
                return NotFound();
            }

            var ServiceOffer = await _context.ServiceOffer
                .Include(o => o.Supplier)
                .FirstOrDefaultAsync(m => m.ServiceOfferID == id);
            if (ServiceOffer == null)
            {
                return NotFound();
            }

            return View(ServiceOffer);
        }









        // POST: ServiceOffers/Delete/5
        [HttpPost("/ServiceOffers/Delete/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supplier")]
        [SwaggerOperation(Summary = "Delete sale offer", Description = "Delete a sale offer.")]
        [SwaggerResponse(200, "Sale offer deleted successfully.")]
        [SwaggerResponse(404, "Sale offer not found.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ServiceOffer == null)
            {
                return Problem("Entity set 'GAPContext.ServiceOffer' is null.");
            }

            var ServiceOffer = await _context.ServiceOffer.FindAsync(id);

            if (ServiceOffer != null)
            {
         
                _context.ServiceOffer.Remove(ServiceOffer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        /*---------------------------------------------------------------*/







        // Helper: no route
        private bool ServiceOfferExists(int id)
        {
            return (_context.ServiceOffer?.Any(e => e.ServiceOfferID == id)).GetValueOrDefault();
        }
    }
}
