﻿using System;
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
        public async Task<IActionResult> Index()
        {
            var gAPContext = _context.OffreVente.Include(o => o.Fournisseur);
            return View(await gAPContext.ToListAsync());
        }
        
        [Authorize(Roles = "RespServiceAchat")]

        // GET: OffreVentes
        public async Task<IActionResult> IndexRespSA()
        {
            var gAPContext = _context.OffreVente.Include(o => o.Fournisseur);
            return View(await gAPContext.ToListAsync());
        }

        [Authorize(Roles = "Fournisseur")]

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
        public IActionResult Create(int demandeAchatId)
        {
            // Retrieve the DemandeAchat object based on the received demandeAchatId
            var demandeAchat = _context.DemandeAchat.Find(demandeAchatId);
           
            if (demandeAchat == null)
            {
                // Handle the case when the DemandeAchat is not found
                return NotFound();
            }

            demandeAchat.IsValid = false;
            _context.Update(demandeAchat);
             _context.SaveChanges();

            var produitsList = _context.Produit
                .Select(p => new SelectListItem
                {
                    Value = p.ProduitID.ToString(),
                    Text = $"P Name: {p.Nom} | NbrPiece : {p.NombrePiece} | Prix Unitaire: {p.PrixUnitaire}"
                })
                .ToList();

            // Create the select list for dropdown menu
            ViewBag.ProduitsList = new SelectList(produitsList, "Value", "Text");

            // Pass the DemandeAchatID to the view model, so it can be submitted back when creating the OffreVente
            ViewBag.DemandeAchatId = demandeAchatId;

            return View();
        }


        // POST: OffreVentes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OffreVente offreVente, int selectedProduitId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _context.Fournisseur.FirstOrDefault(u => u.FournisseurID == userId);
            offreVente.FournisseurId = userId;
            offreVente.Fournisseur = user;

            // Check if the selected product already exists in any existing "OffreVente"
            var existingOffre = _context.OffreVente.Include(o => o.Produits).FirstOrDefault(o => o.Produits.Any(p => p.ProduitID == selectedProduitId));

            if (existingOffre != null)
            {
                // If the product is found in an existing "OffreVente," display an error message to the user.
                ModelState.AddModelError("Produit", "This product is already used in another 'offre de ventes'.");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // The product is not found in any existing "OffreVente," so add it to the current one.
                var selectedProduit = _context.Produit.Find(selectedProduitId);
                if (offreVente.Produits == null)
                {
                    offreVente.Produits = new List<Produit>();
                }
                offreVente.Produits.Add(selectedProduit);

                _context.Add(offreVente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            ViewData["ProduitsWithoutOffreVente"] = new SelectList(produitsWithoutOffreVente, "ProduitID", "Nom");

            return View(offreVente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fournisseur")]
        public async Task<IActionResult> Edit(int id, [Bind("OffreVenteID, PrixTTL, SelectedProduitId")] OffreVente updatedOffreVente)
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
            existingOffreVente.PrixTTL = updatedOffreVente.PrixTTL;

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
