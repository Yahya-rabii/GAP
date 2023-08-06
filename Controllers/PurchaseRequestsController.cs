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
using X.PagedList;

namespace GAP.Controllers
{
    [Authorize]
    public class PurchaseRequestsController : Controller
    {
        private readonly GAPContext _context;

        public PurchaseRequestsController(GAPContext context)
        {
            _context = context;
        }

        // GET: PurchaseRequests
        [Authorize(Roles = "PurchasingDepartmentManager")]

        public async Task<IActionResult> Index(int? page ,string SearchString)
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
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> IndexFour(int? page, string SearchString)
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


        // GET: PurchaseRequests/Details/5
        [Authorize(Roles = "PurchasingDepartmentManager")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PurchaseRequest == null)
            {
                return NotFound();
            }

            var PurchaseRequest = await _context.PurchaseRequest
                .FirstOrDefaultAsync(m => m.PurchaseRequestID == id);
            if (PurchaseRequest == null)
            {
                return NotFound();
            }

            return View(PurchaseRequest);
        }

        // GET: PurchaseRequests/Create
        [Authorize(Roles = "PurchasingDepartmentManager")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: PurchaseRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PurchasingDepartmentManager")]

        public async Task<IActionResult> Create([Bind("PurchaseRequestID,Description,Budget")] PurchaseRequest PurchaseRequest)
        {
            if (ModelState.IsValid)
            {
                PurchaseRequest.CreationDate = DateTime.Now;
                _context.Add(PurchaseRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(PurchaseRequest);
        }

        // GET: PurchaseRequests/Edit/5
        [Authorize(Roles = "PurchasingDepartmentManager")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PurchaseRequest == null)
            {
                return NotFound();
            }

            var PurchaseRequest = await _context.PurchaseRequest.FindAsync(id);
            if (PurchaseRequest == null)
            {
                return NotFound();
            }
            return View(PurchaseRequest);
        }

        // POST: PurchaseRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PurchasingDepartmentManager")]

        public async Task<IActionResult> Edit(int id, [Bind("PurchaseRequestID,Description,Budget")] PurchaseRequest PurchaseRequest)
        {
            if (id != PurchaseRequest.PurchaseRequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    PurchaseRequest.CreationDate = DateTime.Now;
                    _context.Update(PurchaseRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseRequestExists(PurchaseRequest.PurchaseRequestID))
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
            return View(PurchaseRequest);
        }

        // GET: PurchaseRequests/Delete/5
        [Authorize(Roles = "PurchasingDepartmentManager")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PurchaseRequest == null)
            {
                return NotFound();
            }

            var PurchaseRequest = await _context.PurchaseRequest
                .FirstOrDefaultAsync(m => m.PurchaseRequestID == id);
            if (PurchaseRequest == null)
            {
                return NotFound();
            }

            return View(PurchaseRequest);
        }

        // POST: PurchaseRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PurchasingDepartmentManager")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PurchaseRequest == null)
            {
                return Problem("Entity set 'GAPContext.PurchaseRequest'  is null.");
            }
            var PurchaseRequest = await _context.PurchaseRequest.FindAsync(id);
            if (PurchaseRequest != null)
            {
                _context.PurchaseRequest.Remove(PurchaseRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseRequestExists(int id)
        {
          return (_context.PurchaseRequest?.Any(e => e.PurchaseRequestID == id)).GetValueOrDefault();
        }
    }
}
