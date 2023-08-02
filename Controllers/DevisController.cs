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

namespace GAP.Controllers
{
    [Authorize]
    [Authorize(Roles = "RespServiceAchat")]
    public class DevisController : Controller
    {
        private readonly GAPContext _context;

        public DevisController(GAPContext context)
        {
            _context = context;
        }

        // GET: Devis
        public async Task<IActionResult> Index(int? page, string SearchString)
        {


            IQueryable<Devis> Devisiq = from o in _context.Devis.Include(o => o.Fournisseur) select o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                Devisiq = _context.Devis.Include(o => o.Fournisseur).Where(o => o.Fournisseur.Email.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await Devisiq.ToPagedListAsync(pageNumber, pageSize));


        }

        // GET: Devis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Devis == null)
            {
                return NotFound();
            }

            var devis = await _context.Devis
                .Include(d => d.Fournisseur)
                .Include(d=>d.OffreVente)
                .ThenInclude(o=>o.DemandeAchat)
                .Include(d=>d.Produits)
                .FirstOrDefaultAsync(m => m.DevisID == id);

            if (devis == null)
            {
                return NotFound();
            }

          

            return View(devis);
        }




        // GET: Devis/Create

        public IActionResult Create(int OffreVenteID)
        {
            // Retrieve the OffreVente object based on the received OffreVenteID
            var OffreVente = _context.OffreVente.Find(OffreVenteID);

            if (OffreVente == null)
            {
                // Handle the case when the OffreVente is not found
                return NotFound();
            }           
            
            OffreVente.Validite = true;
            _context.Update(OffreVente);
            _context.SaveChangesAsync();

            ViewBag.OffreVenteID = OffreVenteID;

            return View();
        }



        // POST: Devis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int OffreVenteID, [Bind("DevisID,DateReception")] Devis devis)
        {
            if (ModelState.IsValid)
            {
                // Get the ID of the currently logged-in user
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var OffreVente = await _context.OffreVente.FindAsync(OffreVenteID);
                if (OffreVente != null)
                {
                    devis.RespServiceAchatId = userId;
                    devis.OffreVente = OffreVente;
                    devis.OffreVenteID = OffreVenteID;
                    devis.DateCreation = DateTime.Now;

                    // Use ExecuteSqlRawAsync with a parameter to fetch related Produit entities
                    var list = await _context.Produit
                        .FromSqlRaw("SELECT * FROM Produit WHERE OffreVenteId = {0}", OffreVenteID)
                        .ToListAsync();

                    devis.Produits = list;
                    float ?prixTTLpiece=0;
                    int ?Nbrpiece=0;

                    foreach(var p in OffreVente.Produits)
                    {
                        prixTTLpiece += p.NombrePiece * p.PrixUnitaire;
                        Nbrpiece += p.NombrePiece;
                    } 
                    
                    devis.PrixTTL = prixTTLpiece +   OffreVente.unit_profit * Nbrpiece;
                    devis.Ntypeproduits = OffreVente.Produits.Count();
                    devis.FournisseurID = OffreVente.FournisseurId;



                    var  demandeAchat = await _context.DemandeAchat.FindAsync(OffreVente.DemandeAchatId);

                    if (demandeAchat != null)
                    {
                        demandeAchat.IsValid = false;
                    }
                    _context.Update(demandeAchat);

                }

                _context.Add(devis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FournisseurID"] = new SelectList(_context.Fournisseur, "FournisseurID", "Email", devis.FournisseurID);
            return View(devis);
        }



        // GET: Devis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Devis == null)
            {
                return NotFound();
            }

            var devis = await _context.Devis.FindAsync(id);
            if (devis == null)
            {
                return NotFound();
            }
            ViewData["FournisseurID"] = new SelectList(_context.Fournisseur, "FournisseurID", "Email", devis.FournisseurID);
            return View(devis);
        }

        // POST: Devis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DevisID,DateCreation,DateReception,ProduitID,unit_profit,Ntypeproduits,FournisseurID,RespServiceAchatId")] Devis devis)
        {
            if (id != devis.DevisID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DevisExists(devis.DevisID))
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
            ViewData["FournisseurID"] = new SelectList(_context.Fournisseur, "FournisseurID", "Email", devis.FournisseurID);
            return View(devis);
        }

        // GET: Devis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Devis == null)
            {
                return NotFound();
            }

            var devis = await _context.Devis
                .Include(d => d.Fournisseur)
                .FirstOrDefaultAsync(m => m.DevisID == id);
            if (devis == null)
            {
                return NotFound();
            }

            return View(devis);
        }

        // POST: Devis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Devis == null)
            {
                return Problem("Entity set 'GAPContext.Devis'  is null.");
            }
            var devis = await _context.Devis.FindAsync(id);
            if (devis != null)
            {
                _context.Devis.Remove(devis);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DevisExists(int id)
        {
          return (_context.Devis?.Any(e => e.DevisID == id)).GetValueOrDefault();
        }
    }
}
