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
        public async Task<IActionResult> Index()
        {
              return _context.Stock != null ? 
                          View(await _context.Stock.ToListAsync()) :
                          Problem("Entity set 'GAPContext.Stock'  is null.");
        }

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


                int pageSize = 2;
                int pageNumber = page ?? 1;
                return View(await stockItemsWithProductsNotInProjects.ToPagedListAsync(pageNumber, pageSize));
            }

            return View();
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int projectId)
        {
            var product = _context.Product.FirstOrDefault(p => p.ProductID == productId);
            var project = _context.Project.FirstOrDefault(p => p.ProjectID == projectId);

            if (product != null && project != null)
            {
                project.Products.Add(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("IndexProjectManager", new { page = 1 });
        }


        // GET: Stocks/Details/5
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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

        private bool StockExists(int id)
        {
          return (_context.Stock?.Any(e => e.StockID == id)).GetValueOrDefault();
        }
    }
}
