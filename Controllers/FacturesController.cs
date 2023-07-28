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

namespace GAP.Controllers
{

    [Authorize]
    [Authorize(Roles = "RespServiceFinance")]
    public class FacturesController : Controller
    {
        private readonly GAPContext _context;

        public FacturesController(GAPContext context)
        {
            _context = context;
        }

        // GET: Factures
        public async Task<IActionResult> Index()
        {
              return _context.Facture != null ? 
                          View(await _context.Facture.ToListAsync()) :
                          Problem("Entity set 'GAPContext.Facture'  is null.");
        }

        // GET: Factures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Facture == null)
            {
                return NotFound();
            }

            var facture = await _context.Facture
                .FirstOrDefaultAsync(m => m.FactureID == id);
            if (facture == null)
            {
                return NotFound();
            }

            return View(facture);
        }

        // GET: Factures/Create
        public IActionResult Create(int devisId)
        {
            // Store the demandeAchatId in ViewBag or ViewData so that it can be used in the view.
            ViewBag.devisId = devisId;

 


       
            return View();
        }

        // POST: Factures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int devisId, [Bind("FactureID")] Facture facture)
        {

            if (devisId == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)

            {
                
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var devis = _context.Devis.Include(d=>d.Fournisseur).FirstOrDefault(d => d.DevisID == devisId) ;


                facture.DevisID = devisId;
                facture.FournisseurEmail = devis.Fournisseur.Email;
                facture.RespServiceFinanceId = userId;
                facture.Validite = false;
                facture.Prix = (double)devis.PrixTTL;




                var notification = _context.Notification.FirstOrDefault(d => d.UserID == userId);
                if (notification != null)
                {
                    _context.Notification.Remove(notification);
                }


                _context.Add(facture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(facture);
        }

        // GET: Factures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Facture == null)
            {
                return NotFound();
            }

            var facture = await _context.Facture.FindAsync(id);
            if (facture == null)
            {
                return NotFound();
            }
            return View(facture);
        }

        // POST: Factures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FactureID,Prix,FournisseurEmail,Validite,RespServiceFinanceId,DevisID")] Facture facture)
        {
            if (id != facture.FactureID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FactureExists(facture.FactureID))
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
            return View(facture);
        }

        // GET: Factures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Facture == null)
            {
                return NotFound();
            }

            var facture = await _context.Facture
                .FirstOrDefaultAsync(m => m.FactureID == id);
            if (facture == null)
            {
                return NotFound();
            }

            return View(facture);
        }

        // POST: Factures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Facture == null)
            {
                return Problem("Entity set 'GAPContext.Facture'  is null.");
            }
            var facture = await _context.Facture.FindAsync(id);
            if (facture != null)
            {
                _context.Facture.Remove(facture);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FactureExists(int id)
        {
          return (_context.Facture?.Any(e => e.FactureID == id)).GetValueOrDefault();
        }
    }
}
