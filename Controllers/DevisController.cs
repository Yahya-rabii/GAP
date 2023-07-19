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
        public async Task<IActionResult> Index()
        {
            var gAPContext = _context.Devis.Include(d => d.Fournisseur).Include(d => d.Produit);
            return View(await gAPContext.ToListAsync());
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
                .Include(d => d.Produit)
                .FirstOrDefaultAsync(m => m.DevisID == id);
            if (devis == null)
            {
                return NotFound();
            }

            return View(devis);
        }

        // GET: Devis/Create
        public IActionResult Create()
        {
            ViewData["FournisseurID"] = new SelectList(_context.Fournisseur, "FournisseurID", "Email");
            ViewData["ProduitID"] = new SelectList(_context.Produit, "ProduitID", "Nom");
            return View();
        }

        // POST: Devis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DevisID,DateReception,ProduitID,PrixTTL,NombrePiece,FournisseurID,RespServiceAchatId")] Devis devis)
        {

            if (ModelState.IsValid)
            {
                // Get the ID of the currently logged-in user
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                devis.RespServiceAchatId = userId;
                devis.DateCreation = DateTime.Now;
                _context.Add(devis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FournisseurID"] = new SelectList(_context.Fournisseur, "FournisseurID", "Email", devis.FournisseurID);
            ViewData["ProduitID"] = new SelectList(_context.Produit, "ProduitID", "Nom", devis.ProduitID);
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
            ViewData["ProduitID"] = new SelectList(_context.Produit, "ProduitID", "ProduitID", devis.ProduitID);
            return View(devis);
        }

        // POST: Devis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DevisID,DateCreation,DateReception,ProduitID,PrixTTL,NombrePiece,FournisseurID,RespServiceAchatId")] Devis devis)
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
            ViewData["ProduitID"] = new SelectList(_context.Produit, "ProduitID", "ProduitID", devis.ProduitID);
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
                .Include(d => d.Produit)
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
