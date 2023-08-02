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
using Xceed.Document.NET;
using Xceed.Words.NET;

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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Facture == null)
            {
                return NotFound();
            }

            var facture = await _context.Facture
                .FirstOrDefaultAsync(m => m.FactureID == id);

            var devis = await _context.Devis.Include(d=>d.Produits).Include(d => d.Fournisseur)
                .FirstOrDefaultAsync(m => m.DevisID == facture.DevisID);

            if (facture == null || devis == null)
            {
                return NotFound();
            }

            // Load the existing Word document template
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "facture.docx");
            using (DocX document = DocX.Load(templatePath))
            {
                // Replace placeholders in the template with the corresponding data
                document.ReplaceText("Numéro de facture :", "Numéro de facture : #" + DateTime.Now.Date.ToString("yyyyMMdd") + "-" +facture.FactureID.ToString());
                document.ReplaceText("Date de Création :", "Date de Création : " + devis.DateCreation.Date.ToString());
                document.ReplaceText("Nom du Fournisseur :", "Nom du Fournisseur : " + devis.Fournisseur.Nom.ToString());
                document.ReplaceText("Email Fournisseur :", "Email Fournisseur : " + devis.Fournisseur.Email.ToString());
                document.ReplaceText("Adresse Fournisseur :", "Adresse Fournisseur : " + devis.Fournisseur.Adresse.ToString());
                document.ReplaceText("Code Postal Fournisseur :", "Code Postal Fournisseur : " + devis.Fournisseur.CodePostal.ToString());
                document.ReplaceText("Numéro de téléphone Fournisseur :", "Numéro de téléphone Fournisseur : " + devis.Fournisseur.Numtele.ToString());

                // Check if the document has at least one table
                if (document.Tables.Count > 2)
                {
                    // Get the third table in the document
                    Table table = document.Tables[2];

                    float MTotalHT=0;
                    float TotalTVA=0;
                    float TotalTTC=0;


                    // Iterate through each item in devis.Produits and add a row for each item in the table
                    foreach (var produit in devis.Produits)
                    {
                        Row row = table.InsertRow();
                        row.Cells[0].Paragraphs[0].Append(produit.Desc);
                        row.Cells[1].Paragraphs[0].Append(produit.NombrePiece.ToString());
                        row.Cells[2].Paragraphs[0].Append(produit.PrixUnitaire.ToString());
                        row.Cells[3].Paragraphs[0].Append("20%");
                        row.Cells[4].Paragraphs[0].Append(produit.Prixtotal.ToString());

                        MTotalHT += produit.Prixtotal;
                        TotalTVA += produit.Prixtotal * 20 / 100;

                    }

                    // You can add more rows and populate them with data as needed.

                    var offre = _context.OffreVente.Where(o => o.FournisseurId == devis.FournisseurID).FirstOrDefault();

                    TotalTTC = (float)(MTotalHT + TotalTVA + offre.profitTTL);
                    document.ReplaceText("Profit fournisseur :", "Profit fournisseur : " + offre.profitTTL.ToString());
                    document.ReplaceText("Montant Total HT :", "Montant Total HT : " + MTotalHT.ToString());
                    document.ReplaceText("Total TVA :", "Total TVA : " + TotalTVA.ToString());
                    document.ReplaceText("Montant Total TTC :", "Montant Total TTC : " + TotalTTC.ToString()); // Corrected line
                    document.ReplaceText("Montant déjà versé :", "Montant déjà versé : 0"); ;
                    document.ReplaceText("Reste à payer :", "Reste à payer : " + TotalTTC.ToString());

                






                }

                // Save the updated document with the new data
                string dateFormatted = DateTime.Now.Date.ToString("yyyyMMdd");
                string fileName = $"fact-{devis.Fournisseur.Nom}-{dateFormatted}.docx";
                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", fileName);
                document.SaveAs(outputPath);
            }

            // Return the file for download or other processing (e.g., you can use a FileResult)
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
                facture.RespServiceFinanceId = userId;
                facture.Validite = false;




                var notification = _context.NotificationReclamation.FirstOrDefault(d => d.UserID == userId);
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
