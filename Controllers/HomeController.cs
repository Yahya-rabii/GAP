using GAP.Data;
using GAP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;
using System.Security.Claims;
using X.PagedList;

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






        // GET: PurchaseRequests for Supplier, this method will be allowed for the Supplier role
        [HttpGet("/PurchaseRequestsOut")]
        [SwaggerOperation(Summary = "Get purchase requests for suppliers", Description = "Retrieve a list of purchase requests for suppliers.")]
        [SwaggerResponse(200, "List of purchase requests for suppliers retrieved successfully.")]
        public async Task<IActionResult> PurchaseRequestsOut(int? page, string SearchString)
        {


            IQueryable<PurchaseRequest> PurchaseRequestiq = from s in _context.PurchaseRequest
                                                            select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                PurchaseRequestiq = _context.PurchaseRequest.Where(d => d.Description.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await PurchaseRequestiq.ToPagedListAsync(pageNumber, pageSize));
        }



        // GET: PurchaseRequests for Supplier, this method will be allowed for the Supplier role
        [HttpGet("/ServicesRequestsOut")]
        [SwaggerOperation(Summary = "Get purchase requests for suppliers", Description = "Retrieve a list of purchase requests for suppliers.")]
        [SwaggerResponse(200, "List of purchase requests for suppliers retrieved successfully.")]
        public async Task<IActionResult> ServicesRequestsOut(int? page, string SearchString)
        {


            IQueryable<ServiceRequest> ServicesRequestiq = from s in _context.ServiceRequest
                                                           select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                ServicesRequestiq = _context.ServiceRequest.Where(d => d.Title.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await ServicesRequestiq.ToPagedListAsync(pageNumber, pageSize));
        }







        [HttpGet("/OutIndex")]
        [SwaggerOperation(Summary = "Home page", Description = "Display the home page.")]
        [SwaggerResponse(200, "Home page displayed successfully.")]
        public IActionResult OutIndex()
        {


            var totalProducts = _context.Stock
           .SelectMany(s => s.Products)
           .Count();


            var totalUsers = _context.User.Count();
            var totalSuppliers = _context.Supplier.Count();
            var TotalBugs = _context.Reclamation.Count();

            var users = _context.User.Where(u => u.Role != UserRole.Project_Manager && u.Role != UserRole.Supplier).ToList();
            var suppliers = _context.Supplier.ToList();
            var pmanagers = _context.ProjectManager.Where(u => u.Role == UserRole.Project_Manager).Include(u => u.Projects).ToList();
            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalBugs = TotalBugs;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalSuppliers = totalSuppliers;
            ViewBag.Users = users;
            ViewBag.PManagers = pmanagers;
            ViewBag.Supplier = suppliers;
            return View();



        }

        [HttpGet("/")]
        [SwaggerOperation(Summary = "Home page", Description = "Display the home page.")]
        [SwaggerResponse(200, "Home page displayed successfully.")]
        public IActionResult Index()
        {

            var totalProducts = _context.Stock
             .SelectMany(s => s.Products)
             .Count();


            var totalUsers = _context.User.Count();
                var totalSuppliers = _context.Supplier.Count();
                var TotalBugs = _context.Reclamation.Count();

            var users = _context.User.Where(u => u.Role != UserRole.Project_Manager && u.Role != UserRole.Supplier).ToList();
            var suppliers = _context.Supplier.ToList();
                var pmanagers = _context.ProjectManager.Where(u => u.Role == UserRole.Project_Manager).Include(u=>u.Projects).ToList();
                ViewBag.TotalProducts = totalProducts;
                ViewBag.TotalBugs = TotalBugs;
                ViewBag.TotalUsers = totalUsers;
                ViewBag.TotalSuppliers = totalSuppliers;
                ViewBag.Users = users;
                ViewBag.PManagers = pmanagers;
                ViewBag.Supplier = suppliers;

                return View();
            
        }





        [HttpGet("/Privacy")]
        [SwaggerOperation(Summary = "Privacy policy page", Description = "Display the privacy policy page.")]
        [SwaggerResponse(200, "Privacy policy page displayed successfully.")]
        public IActionResult Privacy()
        {
            return View();
        }







        // Action method to handle the "Reply" button click
        [HttpGet("/HandleNotification")]
        [SwaggerOperation(Summary = "Handle notifications", Description = "Handle notifications based on user roles.")]
        [SwaggerResponse(302, "Notification handled successfully.")]
        [SwaggerResponse(200, "Default behavior if user is not in the specified roles.")]
        public IActionResult HandleNotification(int PurchaseQuoteId ,int ServiceQuoteId , int SupplierID , int SaleOfferID)
        {

            // Get the user's role and handle the redirection accordingly
            if (User.IsInRole("QualityTestingDepartmentManager"))
            {
                // Redirect to the Create action in QualityTestReport controller with the PurchaseQuoteId parameter

                if (ServiceQuoteId != 0)
                {
                    return RedirectToAction("Create", "QualityTestReports", new { ServiceQuoteId });


                }
               else
                {
                    return RedirectToAction("Create", "QualityTestReports", new { PurchaseQuoteId });

                }
            }
            else if (User.IsInRole("FinanceDepartmentManager"))
            {
                // Redirect to the Create action in Bill controller with the PurchaseQuoteId parameter
                if(ServiceQuoteId != 0)
                {
                return RedirectToAction("Create", "Bills", new { ServiceQuoteId });


                }
                else
                {
                    return RedirectToAction("Create", "Bills", new { PurchaseQuoteId });

                }
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







        // Action method to handle the "AccessDenied" button click
        [HttpGet("/AccessDenied/{statusCode}")]
        [SwaggerOperation(Summary = "Access denied page", Description = "Display the access denied page.")]
        [SwaggerResponse(200, "Access denied page displayed successfully.")]
        [SwaggerResponse(400, "HTTP 400 Bad Request page displayed.")]
        [SwaggerResponse(401, "HTTP 401 Unauthorized page displayed.")]
        [SwaggerResponse(403, "HTTP 403 Forbidden page displayed.")]
        [SwaggerResponse(404, "HTTP 404 Not Found page displayed.")]
        [SwaggerResponse(500, "HTTP 500 Internal Server Error page displayed.")]
        public IActionResult AccessDenied(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                HttpContext.Response.StatusCode = statusCode.Value;
            }
            else
            {
                HttpContext.Response.StatusCode = 500; // Internal Server Error
            }

            string errorTitle = "";
            string errorDescription = "";

            // Set error information based on the status code
            switch (HttpContext.Response.StatusCode)
            {
                case 400:
                    errorTitle = "HTTP 400 Bad Request";
                    errorDescription = "The request could not be understood by the server due to malformed syntax.";
                    break;
                case 401:
                    errorTitle = "HTTP 401 Unauthorized";
                    errorDescription = "Authentication is required and has failed or has not been provided.";
                    break;
                case 403:
                    errorTitle = "HTTP 403 Forbidden";
                    errorDescription = "Access Denied. You Do Not Have The Permission To Access This Page On This Server.";
                    break;
                case 404:
                    errorTitle = "HTTP 404 Not Found";
                    errorDescription = "The requested page was not found on this server.";
                    break;
                case 500:
                    errorTitle = "HTTP 500 Internal Server Error";
                    errorDescription = "An unexpected condition was encountered by the server and no more specific message is suitable.";
                    break;
                default:
                    errorTitle = "Unknown Error";
                    errorDescription = "An unknown error occurred.";
                    break;
            }

            ViewBag.ErrorTitle = errorTitle;
            ViewBag.ErrorDescription = errorDescription;
            ViewBag.StatusCode = HttpContext.Response.StatusCode;

            return View();
        }


    }
}
