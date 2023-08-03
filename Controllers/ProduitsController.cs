using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using X.PagedList;

namespace GAP.Controllers
{

    [Authorize]
    [Authorize(Roles = "Fournisseur")]
    public class ProduitsController : Controller
    {
        private readonly GAPContext _context;

        public ProduitsController(GAPContext context)
        {
            _context = context;
        }

        // GET: Produits1
        public async Task<IActionResult> Index(int? page)
        {            
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            IQueryable<Produit> iseriq = from p in _context.Produit.Where(p => p.FournisseurId == userId)
                                             select p;


            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));


        }

        // GET: Produits1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Produit == null)
            {
                return NotFound();
            }

            var produit = await _context.Produit
                .FirstOrDefaultAsync(m => m.ProduitID == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // GET: Produits1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produits1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProduitID,PrixUnitaire,Nom,NombrePiece,Desc")] Produit produit)
        {
            if (ModelState.IsValid)
            {

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                produit.FournisseurId = userId;
                produit.Prixtotal = (float)(produit.NombrePiece * produit.PrixUnitaire);

                _context.Add(produit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produit);
        }

        // GET: Produits1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Produit == null)
            {
                return NotFound();
            }

            var produit = await _context.Produit.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }
            return View(produit);
        }

        // POST: Produits1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProduitID,PrixUnitaire,Nom,NombrePiece,Desc")] Produit produit)
        {
            if (id != produit.ProduitID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                    produit.FournisseurId = userId;
                    produit.Prixtotal = (float)(produit.PrixUnitaire * produit.NombrePiece);

                    _context.Update(produit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitExists(produit.ProduitID))
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
            return View(produit);
        }

        // GET: Produits1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Produit == null)
            {
                return NotFound();
            }

            var produit = await _context.Produit
                .FirstOrDefaultAsync(m => m.ProduitID == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // POST: Produits1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Produit == null)
            {
                return Problem("Entity set 'GAPContext.Produit'  is null.");
            }
            var produit = await _context.Produit.FindAsync(id);
            if (produit != null)
            {
                _context.Produit.Remove(produit);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduitExists(int id)
        {
          return (_context.Produit?.Any(e => e.ProduitID == id)).GetValueOrDefault();
        }
    }
}
