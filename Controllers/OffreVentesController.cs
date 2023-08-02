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
using Microsoft.Data.SqlClient;
using X.PagedList;

namespace GAP.Controllers
{
    public class OffreVentesController : Controller
    {
        private readonly GAPContext _context;

        public OffreVentesController(GAPContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Fournisseur")]

        // GET: OffreVentes
        public async Task<IActionResult> Index(int? page, string SearchString)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
       

            IQueryable<OffreVente> OffreVenteiq = from o in _context.OffreVente.Include(o => o.Produits).Include(o => o.Fournisseur).Where(o => o.FournisseurId == userId) select o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                OffreVenteiq = _context.OffreVente
                    .Include(o => o.Fournisseur)
                    .Where(o => o.Fournisseur.Nom.ToLower().Contains(SearchString.ToLower().Trim()));
            }

           
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await OffreVenteiq.ToPagedListAsync(pageNumber, pageSize));

        }
        
        [Authorize(Roles = "RespServiceAchat")]

        // GET: OffreVentes
        public async Task<IActionResult> IndexRespSA(int? page, string SearchString)
        {


            IQueryable<OffreVente> OffreVenteiq = from o in _context.OffreVente.Include(o => o.Fournisseur).Include(o=>o.Produits) select o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                OffreVenteiq = _context.OffreVente.Include(o=>o.DemandeAchat).Where(o => o.DemandeAchat.Description.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await OffreVenteiq.ToPagedListAsync(pageNumber, pageSize));

           

        }

        [Authorize(Roles = "Fournisseur,RespServiceAchat")]

        // GET: OffreVentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OffreVente == null)
            {
                return NotFound();
            }

            var offreVente = await _context.OffreVente
                .Include(o => o.Fournisseur)
                .Include(o => o.Produits) // Include associated products
                .Include(o => o.DemandeAchat) // Include the DemandeAchat
                .FirstOrDefaultAsync(m => m.OffreVenteID == id);

            if (offreVente == null)
            {
                return NotFound();
            }

            return View(offreVente);
        }     
        




        // GET: OffreVentes/Create
        [Authorize(Roles = "Fournisseur")]
        // GET: OffreVentes/Create
        public async Task<IActionResult> Create(int demandeAchatId)
        {
            // Store the demandeAchatId in ViewBag or ViewData so that it can be used in the view.
            ViewBag.DemandeAchatId = demandeAchatId;

 


        var produitsWithoutOffreVente = await GetProductsWithoutOffreVente();
            ViewData["ProduitsWithoutOffreVente"] = new SelectList(produitsWithoutOffreVente, "ProduitID", "Nom");

            return View();

        }

        // POST: OffreVentes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int demandeAchatId, OffreVente offreVente, int selectedProduitId)
        {
            if (demandeAchatId == 0)
            {
                return NotFound();
            }

            var demandeAchat = _context.DemandeAchat.Find(demandeAchatId);

            if (demandeAchat == null)
            {
                return NotFound();
            }


            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _context.Fournisseur.FirstOrDefault(u => u.FournisseurID == userId);
            offreVente.FournisseurId = userId;
            offreVente.Fournisseur = user;
            offreVente.DemandeAchatId = demandeAchatId;
            offreVente.DemandeAchat = demandeAchat;


            int? prds = 0;
            // The product is not found in any existing "OffreVente," so add it to the current one.
            var selectedProduit = _context.Produit.Find(selectedProduitId);
            if (offreVente.Produits == null)
            {
                offreVente.Produits = new List<Produit>();


            }
      

            offreVente.Produits.Add(selectedProduit);
              foreach(var p  in offreVente.Produits)
                {
                    prds += p.NombrePiece;
                }

            offreVente.profitTTL = (double)(offreVente.unit_profit * prds);

            _context.Update(demandeAchat);
            _context.Add(offreVente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        [Authorize(Roles = "RespServiceAchat")]
        public async Task<IActionResult> ValidateOffre(int? id)
        {
            if (id == null || _context.OffreVente == null)
            {
                return NotFound();
            }

            var offreVente = await _context.OffreVente.FindAsync(id);
            if (offreVente == null)
            {
                return NotFound();
            }
            else
            {
                // Update the Validite property to false
                offreVente.Validite = true;

                // Mark the entity as modified and save changes
                _context.Update(offreVente);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("IndexRespSA", "OffreVentes");
        }     
 


        ///////////////////////////////////////////////////////////////////////////////////////////////

        private async Task<List<Produit>> GetProductsWithoutOffreVente()
        {
            // Use ExecuteSqlRawAsync to get all products with OffreVenteId as NULL
            return await _context.Produit.FromSqlRaw("SELECT * FROM Produit WHERE OffreVenteId IS NULL").ToListAsync();
        }
        [Authorize(Roles = "Fournisseur")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offreVente = await _context.OffreVente
                .Include(o => o.Produits)
                .FirstOrDefaultAsync(o => o.OffreVenteID == id);

            if (offreVente == null)
            {
                return NotFound();
            }

            ViewData["FournisseurId"] = new SelectList(_context.Fournisseur, "FournisseurID", "Email", offreVente.FournisseurId);

            var produitsWithoutOffreVente = await GetProductsWithoutOffreVente();
            if (produitsWithoutOffreVente.Count() > 0)
            {
                ViewData["ProduitsWithoutOffreVente"] = new SelectList(produitsWithoutOffreVente, "ProduitID", "Nom");
            }
            else
            {
                // Add the message "No products in your stock" to the list
                var emptyList = new List<Produit>
    {
        new Produit { ProduitID = 0, Nom = "No products in your stock" }
    };
                ViewData["ProduitsWithoutOffreVente"] = new SelectList(emptyList, "ProduitID", "Nom");
            }

            return View(offreVente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fournisseur")]
        public async Task<IActionResult> Edit(int id, [Bind("OffreVenteID, unit_profit, SelectedProduitId")] OffreVente updatedOffreVente)
        {
            // Remove the ModelState check, as it might be causing issues

            // Fetch the existing "OffreVente" record from the database
            var existingOffreVente = await _context.OffreVente
                .Include(o => o.Produits)
                .FirstOrDefaultAsync(o => o.OffreVenteID == id);

            if (existingOffreVente == null)
            {
                return NotFound();
            }

            // Update the existing "OffreVente" with the new data
            existingOffreVente.unit_profit = updatedOffreVente.unit_profit;

            // Find the selected product and add it to the "OffreVente.Produits" list if not already present
            var selectedProduit = await _context.Produit.FindAsync(updatedOffreVente.SelectedProduitId);
            if (selectedProduit != null && !existingOffreVente.Produits.Contains(selectedProduit))
            {
                existingOffreVente.Produits.Add(selectedProduit);
            }

            try
            {
                _context.Update(existingOffreVente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OffreVenteExists(existingOffreVente.OffreVenteID))
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







        // GET: OffreVentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OffreVente == null)
            {
                return NotFound();
            }

            var offreVente = await _context.OffreVente
                .Include(o => o.Fournisseur)
                .FirstOrDefaultAsync(m => m.OffreVenteID == id);
            if (offreVente == null)
            {
                return NotFound();
            }

            return View(offreVente);
        }

        // POST: OffreVentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fournisseur")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OffreVente == null)
            {
                return Problem("Entity set 'GAPContext.OffreVente' is null.");
            }

            var offreVente = await _context.OffreVente.FindAsync(id);

            if (offreVente != null)
            {
                // Set the OffreVenteId to null for associated Produits using raw SQL
                var offreVenteIdParam = new SqlParameter("@OffreVenteId", SqlDbType.Int);
                offreVenteIdParam.Value = id;

                await _context.Database.ExecuteSqlRawAsync("UPDATE Produit SET OffreVenteId = NULL WHERE OffreVenteId = @OffreVenteId", offreVenteIdParam);

                // Remove the OffreVente
                _context.OffreVente.Remove(offreVente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool OffreVenteExists(int id)
        {
          return (_context.OffreVente?.Any(e => e.OffreVenteID == id)).GetValueOrDefault();
        }
    }
}
