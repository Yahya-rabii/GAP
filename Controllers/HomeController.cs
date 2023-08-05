using GAP.Data;
using GAP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace GAP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GAPContext _context;

     
        public HomeController(ILogger<HomeController> logger,GAPContext context)
        {
            _logger = logger;  
            _context = context;
        }

        public IActionResult Index()
        {
           
                var totalProducts = _context.Produit.Count();
                var totalUsers = _context.User.Count();
                var totalSuppliers = _context.Fournisseur.Count();

                var users = _context.User.ToList();
                var suppliers = _context.Fournisseur.ToList();

                ViewBag.TotalProducts = totalProducts;
                ViewBag.TotalUsers = totalUsers;
                ViewBag.TotalFournisseurs = totalSuppliers;
                ViewBag.Users = users;
                ViewBag.Fournisseur = suppliers;

                return View();
            
        }


        public IActionResult Privacy()
        {
            return View();
        }

        // Action method to handle the "Reply" button click
        public IActionResult HandleNotification(int devisId , int FournisseurID , int OffreVenteID)
        {

            // Get the user's role and handle the redirection accordingly
            if (User.IsInRole("RespServiceQualite"))
            {
                // Redirect to the Create action in RapportTestQualite controller with the devisId parameter
                return RedirectToAction("Create", "RapportTestQualites", new { devisId });
            }
            else if (User.IsInRole("RespServiceFinance"))
            {
                // Redirect to the Create action in Facture controller with the devisId parameter
                return RedirectToAction("Create", "Factures", new { devisId });
            }
            else if (User.IsInRole("Fournisseur"))
            {
                return RedirectToAction("Details", "OffreVente", new { OffreVenteID });
            }
            else if (User.IsInRole("Admin"))
            {
                var ID = FournisseurID;
                // Redirect to the Create action in Facture controller with the devisId parameter
                return RedirectToAction("Validate", "Fournisseurs", new { ID });
            }

            // Default behavior if user is not in the specified roles
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
