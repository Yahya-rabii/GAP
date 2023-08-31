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
using Swashbuckle.AspNetCore.Annotations;

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
        [HttpGet("/PurchaseRequests")]
        [Authorize(Roles = "PurchasingDepartmentManager")]
        [SwaggerOperation(Summary = "Get purchase requests for purchasing department manager", Description = "Retrieve a list of purchase requests for purchasing department managers.")]
        [SwaggerResponse(200, "List of purchase requests for purchasing department managers retrieved successfully.")]
        public async Task<IActionResult> Index(int? page ,string SearchString)
        {
           

            IQueryable<PurchaseRequest> PurchaseRequestiq = from s in _context.PurchaseRequest
                                      select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                PurchaseRequestiq = _context.PurchaseRequest.Where(d => d.Description.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await PurchaseRequestiq.ToPagedListAsync(pageNumber, pageSize));


        }




        // GET: PurchaseRequests for Supplier, this method will be allowed for the Supplier role
        [HttpGet("/PurchaseRequests/IndexFour")]
        [Authorize(Roles = "Supplier")]
        [SwaggerOperation(Summary = "Get purchase requests for suppliers", Description = "Retrieve a list of purchase requests for suppliers.")]
        [SwaggerResponse(200, "List of purchase requests for suppliers retrieved successfully.")]
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
        [HttpGet("/PurchaseRequests/Details/{id}")]
        [Authorize(Roles = "PurchasingDepartmentManager")]
        [SwaggerOperation(Summary = "Get details of a purchase request", Description = "Retrieve the details of a purchase request for purchasing department managers.")]
        [SwaggerResponse(200, "Purchase request details retrieved successfully.")]
        [SwaggerResponse(404, "Purchase request not found.")]
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
        [HttpGet("/PurchaseRequests/Create")]
        [Authorize(Roles = "PurchasingDepartmentManager")]
        [SwaggerOperation(Summary = "Show purchase request creation form", Description = "Display the purchase request creation form for purchasing department managers.")]
        [SwaggerResponse(200, "Purchase request creation form displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }






        // POST: PurchaseRequests/Create
        [HttpPost("/PurchaseRequests/Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PurchasingDepartmentManager")]
        [SwaggerOperation(Summary = "Create a new purchase request", Description = "Create a new purchase request with the provided information for purchasing department managers.")]
        [SwaggerResponse(200, "Purchase request created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
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
        [HttpGet("/PurchaseRequests/Edit/{id}")]
        [Authorize(Roles = "PurchasingDepartmentManager")]
        [SwaggerOperation(Summary = "Show purchase request edit form", Description = "Display the purchase request edit form for purchasing department managers.")]
        [SwaggerResponse(200, "Purchase request edit form displayed successfully.")]
        [SwaggerResponse(404, "Purchase request not found.")]
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
        [HttpPost("/PurchaseRequests/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PurchasingDepartmentManager")]
        [SwaggerOperation(Summary = "Edit a purchase request", Description = "Edit an existing purchase request with the provided information for purchasing department managers.")]
        [SwaggerResponse(200, "Purchase request edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Purchase request not found.")]
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
        [HttpGet("/PurchaseRequests/Delete/{id}")]
        [Authorize(Roles = "PurchasingDepartmentManager")]
        [SwaggerOperation(Summary = "Show purchase request delete confirmation", Description = "Display the purchase request delete confirmation for purchasing department managers.")]
        [SwaggerResponse(200, "Purchase request delete confirmation displayed successfully.")]
        [SwaggerResponse(404, "Purchase request not found.")]
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
        [HttpPost("/PurchaseRequests/Delete/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PurchasingDepartmentManager")]
        [SwaggerOperation(Summary = "Delete a purchase request", Description = "Delete an existing purchase request for purchasing department managers.")]
        [SwaggerResponse(200, "Purchase request deleted successfully.")]
        [SwaggerResponse(404, "Purchase request not found.")]
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








        /*---------------------------------------------------------------*/





        // Helper: no route
        private bool PurchaseRequestExists(int id)
        {
          return (_context.PurchaseRequest?.Any(e => e.PurchaseRequestID == id)).GetValueOrDefault();
        }



    }
}
