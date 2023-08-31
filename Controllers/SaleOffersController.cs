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
    public class SaleOffersController : Controller
    {
        private readonly GAPContext _context;

        public SaleOffersController(GAPContext context)
        {
            _context = context;
        }


        // GET: SaleOffers
        [HttpGet("/SaleOffers")]
        [Authorize(Roles = "Supplier")]
        [SwaggerOperation(Summary = "Get a list of sale offers", Description = "Retrieve a list of sale offers.")]
        [SwaggerResponse(200, "List of sale offers retrieved successfully.")]
        public async Task<IActionResult> Index(int? page, string SearchString)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            IQueryable<SaleOffer> SaleOfferiq = from o in _context.SaleOffer.Include(o => o.Products).Include(o => o.Supplier).Where(o => o.SupplierId == userId) select o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                SaleOfferiq = SaleOfferiq
                    .Include(o => o.Supplier)
                    .Where(o => o.Supplier.CompanyName.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var pagedSaleOffers = await SaleOfferiq.ToPagedListAsync(pageNumber, pageSize);

            // Step 1: Get SaleOffer IDs from PurchaseQuote
            var SaleOfferIdsfromPurchaseQuote = await _context.PurchaseQuote
                .Select(PurchaseQuote => PurchaseQuote.SaleOfferID)
                .ToListAsync();

            // Step 2: Get offre de vente with PurchaseQuote
            var SaleOfferWithPurchaseQuote = await _context.SaleOffer
                .Where(owd => SaleOfferIdsfromPurchaseQuote.Contains(owd.SaleOfferID))
                .ToListAsync();

            // Step 3: Get SaleOffer IDs without PurchaseQuote
            var SaleOffersWithoutPurchaseQuote = await _context.SaleOffer
                .Where(owtd => !SaleOfferIdsfromPurchaseQuote.Contains(owtd.SaleOfferID))
                .ToListAsync();

            // Step 4: Check if SaleOffer without PurchaseQuote that I have already got have the same PurchaseRequestid as SaleOffer with PurchaseQuote I have already got. If so, delete SaleOffer without PurchaseQuote
            foreach (var offreWithoutPurchaseQuote in SaleOffersWithoutPurchaseQuote)
            {
                foreach (var offreWithPurchaseQuote in SaleOfferWithPurchaseQuote)
                {
                    if (offreWithoutPurchaseQuote.PurchaseRequestId == offreWithPurchaseQuote.PurchaseRequestId)
                    {
                        NotificationSupplier notificationSupplier = new NotificationSupplier();
                        notificationSupplier.NotificationTitle = "offre de vente refuse";
                        notificationSupplier.SupplierID = offreWithoutPurchaseQuote.SupplierId;
                        notificationSupplier.SaleOfferID = offreWithoutPurchaseQuote.SaleOfferID;

                        _context.NotificationSupplier.Add(notificationSupplier);

                        // Update products associated with this SaleOffer without PurchaseQuote to set SaleOfferId to null using ExecuteSqlRawAsync
                       // var SaleOfferIdParam = new SqlParameter("@SaleOfferId", offreWithoutPurchaseQuote.SaleOfferID);
                        //await _context.Database.ExecuteSqlRawAsync("UPDATE Product SET SaleOfferId = NULL WHERE SaleOfferId = @SaleOfferId", SaleOfferIdParam);

                        // Remove the SaleOffer without PurchaseQuote
                       // _context.SaleOffer.Remove(offreWithoutPurchaseQuote);
                        break; // Exit the inner loop once a match is found and deletion is performed
                    }
                }
            }

            await _context.SaveChangesAsync(); // Save the changes to the database

            return View(pagedSaleOffers);





        }






        // GET: SaleOffers
        [HttpGet("/SaleOffers/IndexRespSA")]
        [SwaggerOperation(Summary = "Get sale offers for purchasing department manager", Description = "Retrieve sale offers for the purchasing department manager.")]
        [SwaggerResponse(200, "Sale offers retrieved successfully.")]
        [Authorize(Roles = "PurchasingDepartmentManager")]
        public async Task<IActionResult> IndexRespSA(int? page, string SearchString)
        {


            IQueryable<SaleOffer> SaleOfferiq = from o in _context.SaleOffer.Include(o => o.Supplier).Include(o=>o.Products) select o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                SaleOfferiq = _context.SaleOffer.Include(o=>o.PurchaseRequest).Where(o => o.PurchaseRequest.Description.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await SaleOfferiq.ToPagedListAsync(pageNumber, pageSize));

           

        }




        // GET: SaleOffers/Details/5
        [HttpGet("/SaleOffers/Details/{id}")]
        [SwaggerOperation(Summary = "Get sale offer details", Description = "Retrieve details of a sale offer.")]
        [SwaggerResponse(200, "Sale offer details retrieved successfully.")]
        [SwaggerResponse(404, "Sale offer not found.")]
        [Authorize(Roles = "Supplier,PurchasingDepartmentManager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SaleOffer == null)
            {
                return NotFound();
            }

            var SaleOffer = await _context.SaleOffer
                .Include(o => o.Supplier)
                .Include(o => o.Products) // Include associated products
                .Include(o => o.PurchaseRequest) // Include the PurchaseRequest
                .FirstOrDefaultAsync(m => m.SaleOfferID == id);

            if (SaleOffer == null)
            {
                return NotFound();
            }

            return View(SaleOffer);
        }






        // GET: SaleOffers/Create
        [Authorize(Roles = "Supplier")]
        [HttpGet("/SaleOffers/Create")]
        [SwaggerOperation(Summary = "Show sale offer creation form", Description = "Display the sale offer creation form.")]
        [SwaggerResponse(200, "Sale offer creation form displayed successfully.")]
        public async Task<IActionResult> Create(int PurchaseRequestId)
        {
            // Store the PurchaseRequestId in ViewBag so that it can be used in the view.
            ViewBag.PurchaseRequestId = PurchaseRequestId;

            var productsWithoutSaleOffer = await GetProductsWithoutSaleOffer();
            ViewBag.ProductsWithoutSaleOffer = new SelectList(productsWithoutSaleOffer, "ProductID", "Name"); 

            return View();
        }





        // POST: SaleOffers/Create
        [HttpPost("/SaleOffers/Create")]
        [Authorize(Roles = "Supplier")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new sale offer", Description = "Create a new sale offer with the provided information.")]
        [SwaggerResponse(200, "Sale offer created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Create(int PurchaseRequestId, SaleOffer SaleOffer, int selectedProductId)
        {
            if (PurchaseRequestId == 0)
            {
                return NotFound();
            }

            var PurchaseRequest = _context.PurchaseRequest.Find(PurchaseRequestId);

            if (PurchaseRequest == null)
            {
                return NotFound();
            }


            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _context.Supplier.FirstOrDefault(u => u.UserID == userId);
            SaleOffer.SupplierId = userId;
            SaleOffer.Supplier = user;
            SaleOffer.PurchaseRequestId = PurchaseRequestId;
            SaleOffer.PurchaseRequest = PurchaseRequest;


            int? prds = 0;
            // The product is not found in any existing "SaleOffer," so add it to the current one.
            var selectedProduct = _context.Product.Find(selectedProductId);
            if (SaleOffer.Products == null)
            {
                SaleOffer.Products = new List<Product>();


            }
      

            SaleOffer.Products.Add(selectedProduct);
              foreach(var p  in SaleOffer.Products)
                {
                    prds += p.ItemsNumber;
                }

            SaleOffer.TotalProfit = (double)(SaleOffer.UnitProfit * prds);

            _context.Update(PurchaseRequest);
            _context.Add(SaleOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }






        // POST: SaleOffers/ValidateOffre
        [Authorize(Roles = "PurchasingDepartmentManager")]
        [HttpPost("/SaleOffers/ValidateOffre/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Validate sale offer", Description = "Validate a sale offer.")]
        [SwaggerResponse(200, "Sale offer validated successfully.")]
        [SwaggerResponse(404, "Sale offer not found.")]
        public async Task<IActionResult> ValidateOffre(int? id)
        {
            if (id == null || _context.SaleOffer == null)
            {
                return NotFound();
            }

            var SaleOffer = await _context.SaleOffer.FindAsync(id);
            if (SaleOffer == null)
            {
                return NotFound();
            }
            else
            {
                // Update the Validity property to false
                SaleOffer.Validity = true;

                // Mark the entity as modified and save changes
                _context.Update(SaleOffer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("IndexRespSA", "SaleOffers");
        }     
 



        








        [Authorize(Roles = "Supplier")]
        [HttpGet("/SaleOffers/Edit/{id}")]
        [SwaggerOperation(Summary = "Show sale offer editing form", Description = "Display the form for editing a sale offer's information.")]
        [SwaggerResponse(200, "Sale offer editing form displayed successfully.")]
        [SwaggerResponse(404, "Sale offer not found.")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SaleOffer = await _context.SaleOffer
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.SaleOfferID == id);

            if (SaleOffer == null)
            {
                return NotFound();
            }

            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierID", "Email", SaleOffer.SupplierId);



            var productsWithoutSaleOffer = await GetProductsWithoutSaleOffer();




            if (productsWithoutSaleOffer.Count() > 0)
            {
                ViewBag.ProductsWithoutSaleOffer = new SelectList(productsWithoutSaleOffer, "ProductID", "Name");
            }
            else
            {
                // Add the message "No products in your stock" to the list
                var emptyList = new List<Product>
    {
        new Product { ProductID = 0, Name = "No products in your stock" }
    };
                ViewBag.ProductsWithoutSaleOffer = new SelectList(emptyList, "ProductID", "CompanyName");
            }

            return View(SaleOffer);
        }










        [HttpPost("/SaleOffers/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supplier")]
        [SwaggerOperation(Summary = "Edit sale offer information", Description = "Edit the information of a sale offer.")]
        [SwaggerResponse(200, "Sale offer information edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Sale offer not found.")]
        public async Task<IActionResult> Edit(int id, [Bind("SaleOfferID, UnitProfit, SelectedProductId")] SaleOffer updatedSaleOffer)
        {
            // Remove the ModelState check, as it might be causing issues

            // Fetch the existing "SaleOffer" record from the database
            var existingSaleOffer = await _context.SaleOffer
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.SaleOfferID == id);

            if (existingSaleOffer == null)
            {
                return NotFound();
            }

            // Update the existing "SaleOffer" with the new data
            existingSaleOffer.UnitProfit = updatedSaleOffer.UnitProfit;

            // Find the selected product and add it to the "SaleOffer.Products" list if not already present
            var selectedProduct = await _context.Product.FindAsync(updatedSaleOffer.SelectedProductId);
            if (selectedProduct != null && !existingSaleOffer.Products.Contains(selectedProduct))
            {
                existingSaleOffer.Products.Add(selectedProduct);
            }

            try
            {
                _context.Update(existingSaleOffer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleOfferExists(existingSaleOffer.SaleOfferID))
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








        // GET: SaleOffers/Delete/5
        [HttpGet("/SaleOffers/Delete/{id}")]
        [SwaggerOperation(Summary = "Show sale offer deleting form", Description = "Display the form for deleting a sale offer's information.")]
        [SwaggerResponse(200, "Sale offer deleting form displayed successfully.")]
        [SwaggerResponse(404, "Sale offer not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SaleOffer == null)
            {
                return NotFound();
            }

            var SaleOffer = await _context.SaleOffer
                .Include(o => o.Supplier)
                .FirstOrDefaultAsync(m => m.SaleOfferID == id);
            if (SaleOffer == null)
            {
                return NotFound();
            }

            return View(SaleOffer);
        }









        // POST: SaleOffers/Delete/5
        [HttpPost("/SaleOffers/Delete/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supplier")]
        [SwaggerOperation(Summary = "Delete sale offer", Description = "Delete a sale offer.")]
        [SwaggerResponse(200, "Sale offer deleted successfully.")]
        [SwaggerResponse(404, "Sale offer not found.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SaleOffer == null)
            {
                return Problem("Entity set 'GAPContext.SaleOffer' is null.");
            }

            var SaleOffer = await _context.SaleOffer.FindAsync(id);

            if (SaleOffer != null)
            {
                // Set the SaleOfferId to null for associated Products using raw SQL
                var SaleOfferIdParam = new SqlParameter("@SaleOfferId", SqlDbType.Int);
                SaleOfferIdParam.Value = id;

                await _context.Database.ExecuteSqlRawAsync("UPDATE Product SET SaleOfferId = NULL WHERE SaleOfferId = @SaleOfferId", SaleOfferIdParam);

                // Remove the SaleOffer
                _context.SaleOffer.Remove(SaleOffer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        /*---------------------------------------------------------------*/





        // Helper: no route
        private async Task<List<Product>> GetProductsWithoutSaleOffer()
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Use ExecuteSqlRawAsync to get products without SaleOfferId and with SupplierId equal to userId
            return await _context.Product.FromSqlRaw("SELECT * FROM Product WHERE SaleOfferId IS NULL AND SupplierId = {0}", userId).ToListAsync();
        }




        // Helper: no route
        private bool SaleOfferExists(int id)
        {
          return (_context.SaleOffer?.Any(e => e.SaleOfferID == id)).GetValueOrDefault();
        }
    }
}
