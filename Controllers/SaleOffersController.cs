using GAP.Data;
using GAP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        [Authorize(Roles = "Supplier")]

        // GET: SaleOffers
        // GET: SaleOffers
        public async Task<IActionResult> Index(int? page, string SearchString)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            IQueryable<SaleOffer> SaleOfferiq = from o in _context.SaleOffer.Include(o => o.Products).Include(o => o.Supplier).Where(o => o.SupplierId == userId) select o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                SaleOfferiq = SaleOfferiq
                    .Include(o => o.Supplier)
                    .Where(o => o.Supplier.Name.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
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





        [Authorize(Roles = "PurchasingDepartmentManager")]

        // GET: SaleOffers
        public async Task<IActionResult> IndexRespSA(int? page, string SearchString)
        {


            IQueryable<SaleOffer> SaleOfferiq = from o in _context.SaleOffer.Include(o => o.Supplier).Include(o=>o.Products) select o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                SaleOfferiq = _context.SaleOffer.Include(o=>o.PurchaseRequest).Where(o => o.PurchaseRequest.Description.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await SaleOfferiq.ToPagedListAsync(pageNumber, pageSize));

           

        }

        [Authorize(Roles = "Supplier,PurchasingDepartmentManager")]

        // GET: SaleOffers/Details/5
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
        // GET: SaleOffers/Create
        public async Task<IActionResult> Create(int PurchaseRequestId)
        {
            // Store the PurchaseRequestId in ViewBag or ViewData so that it can be used in the view.
            ViewBag.PurchaseRequestId = PurchaseRequestId;

 


        var ProductsWithoutSaleOffer = await GetProductsWithoutSaleOffer();
            ViewData["ProductsWithoutSaleOffer"] = new SelectList(ProductsWithoutSaleOffer, "ProductID", "Name");

            return View();

        }

        // POST: SaleOffers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            var user = _context.Supplier.FirstOrDefault(u => u.SupplierID == userId);
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





        [Authorize(Roles = "PurchasingDepartmentManager")]
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
 


        ///////////////////////////////////////////////////////////////////////////////////////////////

        private async Task<List<Product>> GetProductsWithoutSaleOffer()
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Use ExecuteSqlRawAsync to get products without SaleOfferId and with SupplierId equal to userId
            return await _context.Product.FromSqlRaw("SELECT * FROM Product WHERE SaleOfferId IS NULL AND SupplierId = {0}", userId).ToListAsync();
        }
  




        [Authorize(Roles = "Supplier")]
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

            var ProductsWithoutSaleOffer = await GetProductsWithoutSaleOffer();
            if (ProductsWithoutSaleOffer.Count() > 0)
            {
                ViewData["ProductsWithoutSaleOffer"] = new SelectList(ProductsWithoutSaleOffer, "ProductID", "Name");
            }
            else
            {
                // Add the message "No products in your stock" to the list
                var emptyList = new List<Product>
    {
        new Product { ProductID = 0, Name = "No products in your stock" }
    };
                ViewData["ProductsWithoutSaleOffer"] = new SelectList(emptyList, "ProductID", "Name");
            }

            return View(SaleOffer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supplier")]
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




        ///////////////////////////////////////////////////////////////////////////////////////////////







        // GET: SaleOffers/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supplier")]
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


        private bool SaleOfferExists(int id)
        {
          return (_context.SaleOffer?.Any(e => e.SaleOfferID == id)).GetValueOrDefault();
        }
    }
}
