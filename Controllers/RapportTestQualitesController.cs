﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using System.Security.Claims;

namespace GAP.Controllers
{
    public class RapportTestQualitesController : Controller
    {
        private readonly GAPContext _context;

        public RapportTestQualitesController(GAPContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var rapportTestQualites = await _context.RapportTestQualite.ToListAsync();

            // Load the related Devis data (Produit and Fournisseur)
            var devisList = await _context.Devis
                .Include(d => d.Produits)
                .Include(d => d.Fournisseur)
                .ToListAsync();

            // Create a ViewModel list by combining RapportTestQualite and Devis information
            var viewModelList = rapportTestQualites.Select(rtq => new RapportTestQualiteViewModel
            {
                RapportTestQualite = rtq,
                Devis = devisList.FirstOrDefault(d => d.DevisID == rtq.DevisId)
            }).ToList();

            // Create the select list for dropdown menu
            // Assuming DevisID is an int property in the Devis model
            ViewBag.DevisList = new SelectList(devisList, "DevisID", "DisplayText");

            return View(viewModelList);
        }



        // GET: RapportTestQualites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RapportTestQualite == null)
            {
                return NotFound();
            }

            var rapportTestQualite = await _context.RapportTestQualite
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rapportTestQualite == null)
            {
                return NotFound();
            }

            return View(rapportTestQualite);
        }

        // GET: RapportTestQualite/Create
        public IActionResult Create(int devisId)
        {
            // Fetch the list of all devis with corresponding product and supplier names
            var devisList = _context.Devis.Where(d => d.DevisID == devisId)
               .Include(d => d.Fournisseur)
               .Select(d => new SelectListItem
               {
                   Value = d.DevisID.ToString(),
                   Text = $"devis: {d.DevisID} | provider id : {d.Fournisseur.Email}" // Combine product name and supplier email
               })
               .ToList();

            // Create the select list for dropdown menu
            ViewBag.DevisList = new SelectList(devisList, "Value", "Text");
            ViewBag.devisId = devisId;

            return View();
        }

        // POST: RapportTestQualite/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int devisId,  RapportTestQualite rapportTestQualite)
        {
            if (ModelState.IsValid)
            {

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                rapportTestQualite.DevisId = devisId;
                rapportTestQualite.RespServiceQualiteId = userId;


                if (rapportTestQualite.ValiditeNbrPiece && rapportTestQualite.ValiditeEtat && rapportTestQualite.ValiditeFonctionnement)
                {

                    var devis = _context.Devis.Where(f=>f.DevisID ==  devisId).FirstOrDefault();
                    var facture  = _context.Facture.Where(f=>f.DevisID ==  devisId).FirstOrDefault();
                    var fournisseur  = _context.Fournisseur.Where(f=>f.FournisseurID==devis.FournisseurID).FirstOrDefault();
                    
                    if (facture != null && fournisseur !=null)
                    {
                        facture.Validite =  true;
                        fournisseur.NombreTransaction++;
                        _context.Update(facture);
                        _context.Update(fournisseur);

                    }

                }
                else
                {
                    var devis = _context.Devis.Include(d=>d.Fournisseur).Where(d=>d.DevisID==devisId).FirstOrDefault();
                    Sanction sanction = new Sanction();

                    sanction.SanctionTitle = "Unvalalid Rapport Test Qualite";
                    sanction.SanctionDescription = "un des normes de qualites de produit est invalid";
                    sanction.FournisseurId = devis.FournisseurID;
                    _context.Sanction.Add(sanction);





                }



                var notification = _context.NotificationReclamation.FirstOrDefault(d => d.UserID == userId);
                if (notification != null)
                {
                    _context.Notification.Remove(notification);
                }


                // Save the RapportTestQualite object to the database
                _context.RapportTestQualite.Add(rapportTestQualite);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, refill the dropdown list and return the view with validation errors.
            var devisList = _context.Devis
                .Include(d => d.Fournisseur)
                .Select(d => new
                {
                    DevisID = d.DevisID,
                    DevisFournisseurName = d.Fournisseur.Email
                })
                .ToList();

            // Create the select list for dropdown menu
            ViewBag.DevisList = new SelectList(devisList, "DevisID", "DevisFournisseurName");

            return View(rapportTestQualite);
        }

        // GET: RapportTestQualites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RapportTestQualite == null)
            {
                return NotFound();
            }

            var rapportTestQualite = await _context.RapportTestQualite.FindAsync(id);
            if (rapportTestQualite == null)
            {
                return NotFound();
            }
            return View(rapportTestQualite);
        }

        // POST: RapportTestQualites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ValiditeEtat,ValiditeNbrPiece,ValiditeFonctionnement,RespServiceQualiteId,DevisId")] RapportTestQualite rapportTestQualite)
        {
            if (id != rapportTestQualite.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rapportTestQualite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RapportTestQualiteExists(rapportTestQualite.Id))
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
            return View(rapportTestQualite);
        }

        // GET: RapportTestQualites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RapportTestQualite == null)
            {
                return NotFound();
            }

            var rapportTestQualite = await _context.RapportTestQualite
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rapportTestQualite == null)
            {
                return NotFound();
            }

            return View(rapportTestQualite);
        }

        // POST: RapportTestQualites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RapportTestQualite == null)
            {
                return Problem("Entity set 'GAPContext.RapportTestQualite'  is null.");
            }
            var rapportTestQualite = await _context.RapportTestQualite.FindAsync(id);
            if (rapportTestQualite != null)
            {
                _context.RapportTestQualite.Remove(rapportTestQualite);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RapportTestQualiteExists(int id)
        {
          return (_context.RapportTestQualite?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
