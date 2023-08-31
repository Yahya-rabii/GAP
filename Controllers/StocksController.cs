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
using X.PagedList;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Controllers
{
    public class StocksController : Controller
    {
        private readonly GAPContext _context;

        public StocksController(GAPContext context)
        {
            _context = context;
        }






        // GET: Stocks
        [HttpGet("/Stocks")]
        [SwaggerOperation(Summary = "Get a list of stocks", Description = "Retrieve a list of stocks.")]
        [SwaggerResponse(200, "List of stocks retrieved successfully.")]
        public async Task<IActionResult> Index(string SearchString, int? page)
        {
            IQueryable<Stock> iseriq = from s in _context.Stock.Include(s=>s.Products)
                                       select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                iseriq = _context.Stock
                    .Include(s => s.Products)
                    .Where(s => s.Products.Any(p => p.Name.ToLower().Contains(SearchString.ToLower().Trim())))
                    ;
            }


            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        }









        // GET: Stocks
        [HttpGet("/Stocks/IndexProjectManager")]
        [SwaggerOperation(Summary = "Get stocks for project manager", Description = "Retrieve stocks for a project manager.")]
        [SwaggerResponse(200, "Stocks retrieved successfully.")]
        public async Task<IActionResult> IndexProjectManager(int? page)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var projectManager = _context.ProjectManager
                .Include(pm => pm.Projects)
                .ThenInclude(p=>p.Products)
                .Where(pm => pm.UserID == userId).FirstOrDefault();

            if (projectManager?.Projects != null)
            {
                var projects = projectManager.Projects.Select(p => new
                {
                    ProjectID = p.ProjectID,
                    ProjectName = p.Name
                }).ToList();

                ViewBag.Projects = projects;

                var productIdsInProjects = projectManager.Projects
                    .SelectMany(p => p.Products.Select(product => product.ProductID))
                    .ToList();

                // Get the Stock items that have products not in any project's products list
                var stockItemsWithProductsNotInProjects = _context.Stock.Include(pm => pm.Products)
                    .Where(stock => stock.Products.Any(product => !productIdsInProjects.Contains(product.ProductID)))
                    .ToList();


                int pageSize = 5;
                int pageNumber = page ?? 1;
                return View(await stockItemsWithProductsNotInProjects.ToPagedListAsync(pageNumber, pageSize));
            }

            return View();
        }








        // Post: Add
        [HttpPost("/Stocks/Add")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Add product to project", Description = "Add a product to a project.")]
        [SwaggerResponse(302, "Product added to project.")]
        [SwaggerResponse(404, "Product or project not found.")]
        public async Task<IActionResult> Add(int productId, int projectId)
        {
            var product = _context.Product.FirstOrDefault(p => p.ProductID == productId);
            var project = _context.Project.FirstOrDefault(p => p.ProjectID == projectId);

            if (product != null && project != null)
            {
                project.Products.Add(product);
                var stock = _context.Stock.FirstOrDefault(); // Adjust this part based on your logic to get the appropriate stock entity
                stock.Products.Remove(product); // Remove from stock.products list
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("IndexProjectManager", new { page = 1 });
        }








        // GET: Stocks/Details/5
        [HttpGet("/Stocks/Details/{id}")]
        [SwaggerOperation(Summary = "Get stock details", Description = "Retrieve details of a stock.")]
        [SwaggerResponse(200, "Stock details retrieved successfully.")]
        [SwaggerResponse(404, "Stock not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stock == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .FirstOrDefaultAsync(m => m.StockID == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }








        // GET: Stocks/Create
        [HttpGet("/Stocks/Create")]
        [SwaggerOperation(Summary = "Show stock creation form", Description = "Display the stock creation form.")]
        [SwaggerResponse(200, "Stock creation form displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }








        // POST: Stocks/Create
        [HttpPost("/Stocks/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new stock", Description = "Create a new stock with the provided information.")]
        [SwaggerResponse(200, "Stock created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Create([Bind("StockID")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stock);
        }









        // GET: Stocks/Edit/5
        [HttpGet("/Stocks/Edit/{id}")]
        [SwaggerOperation(Summary = "Show stock editing form", Description = "Display the form for editing a stock's information.")]
        [SwaggerResponse(200, "Stock editing form displayed successfully.")]
        [SwaggerResponse(404, "Stock not found.")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stock == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return View(stock);
        }









        // POST: Stocks/Edit/5
        [HttpPost("/Stocks/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit stock information", Description = "Edit the information of a stock.")]
        [SwaggerResponse(200, "Stock information edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Stock not found.")]
        public async Task<IActionResult> Edit(int id, [Bind("StockID")] Stock stock)
        {
            if (id != stock.StockID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.StockID))
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
            return View(stock);
        }








        // GET: Stocks/Delete/5
        [HttpGet("/Stocks/Delete/{id}")]
        [SwaggerOperation(Summary = "Show stock deleting form", Description = "Display the form for deleting a stock's information.")]
        [SwaggerResponse(200, "Stock deleting form displayed successfully.")]
        [SwaggerResponse(404, "Stock not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stock == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .FirstOrDefaultAsync(m => m.StockID == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }






        // POST: Stocks/Delete/5
        [HttpPost("/Stocks/Delete/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete stock", Description = "Delete a stock.")]
        [SwaggerResponse(200, "Stock deleted successfully.")]
        [SwaggerResponse(404, "Stock not found.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stock == null)
            {
                return Problem("Entity set 'GAPContext.Stock'  is null.");
            }
            var stock = await _context.Stock.FindAsync(id);
            if (stock != null)
            {
                _context.Stock.Remove(stock);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        /*---------------------------------------------------------------*/




        // Helper: no route
        private bool StockExists(int id)
        {
          return (_context.Stock?.Any(e => e.StockID == id)).GetValueOrDefault();
        }
    }
}
