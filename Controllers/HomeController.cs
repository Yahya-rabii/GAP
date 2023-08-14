using GAP.Data;
using GAP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var totalProducts = _context.Stock
             .SelectMany(s => s.Products)
             .Count();


            var totalUsers = _context.User.Count();
                var totalSuppliers = _context.Supplier.Count();

                var users = _context.User.Where(u=>u.Role != UserRole.Project_Manager).ToList();
                var suppliers = _context.Supplier.ToList();
                var pmanagers = _context.ProjectManager.Where(u => u.Role == UserRole.Project_Manager).Include(u=>u.Projects).ToList();
                ViewBag.TotalProducts = totalProducts;
                ViewBag.TotalUsers = totalUsers;
                ViewBag.TotalSuppliers = totalSuppliers;
                ViewBag.Users = users;
                ViewBag.PManagers = pmanagers;
                ViewBag.Supplier = suppliers;

                return View();
            
        }


        public IActionResult Privacy()
        {
            return View();
        }

        // Action method to handle the "Reply" button click
        public IActionResult HandleNotification(int PurchaseQuoteId , int SupplierID , int SaleOfferID)
        {

            // Get the user's role and handle the redirection accordingly
            if (User.IsInRole("QualityTestingDepartmentManager"))
            {
                // Redirect to the Create action in QualityTestReport controller with the PurchaseQuoteId parameter
                return RedirectToAction("Create", "QualityTestReports", new { PurchaseQuoteId });
            }
            else if (User.IsInRole("FinanceDepartmentManager"))
            {
                // Redirect to the Create action in Bill controller with the PurchaseQuoteId parameter
                return RedirectToAction("Create", "Bills", new { PurchaseQuoteId });
            }
            else if (User.IsInRole("Supplier"))
            {
                return RedirectToAction("Details", "SaleOffer", new { SaleOfferID });
            }
            else if (User.IsInRole("Admin"))
            {
                var ID = SupplierID;
                // Redirect to the Create action in Bill controller with the PurchaseQuoteId parameter
                return RedirectToAction("Validate", "Suppliers", new { ID });
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
