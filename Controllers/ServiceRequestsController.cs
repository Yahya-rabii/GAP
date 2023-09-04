using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace GAP.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly GAPContext _context;

        public ServiceRequestsController(GAPContext context)
        {
            _context = context;
        }

        // GET: ServiceRequests
        [HttpGet("/ServiceRequests")]
        [Authorize(Roles = "PurchasingDepartmentManager")]
        [SwaggerOperation(Summary = "Get purchase requests for purchasing department manager", Description = "Retrieve a list of purchase requests for purchasing department managers.")]
        [SwaggerResponse(200, "List of purchase requests for purchasing department managers retrieved successfully.")]
        public async Task<IActionResult> Index(int? page, string SearchString)
        {


            IQueryable<ServiceRequest> ServiceRequeststiq = from s in _context.ServiceRequest
                                                            select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                ServiceRequeststiq = _context.ServiceRequest.Where(d => d.Description.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await ServiceRequeststiq.ToPagedListAsync(pageNumber, pageSize));


        }





        // GET: PurchaseRequests for Supplier, this method will be allowed for the Supplier role
        [HttpGet("/ServiceRequests/IndexFour")]
        [Authorize(Roles = "Supplier")]
        [SwaggerOperation(Summary = "Get purchase requests for suppliers", Description = "Retrieve a list of purchase requests for suppliers.")]
        [SwaggerResponse(200, "List of purchase requests for suppliers retrieved successfully.")]
        public async Task<IActionResult> IndexFour(int? page, string SearchString)
        {


            IQueryable<ServiceRequest> ServiceRequeststiq = from s in _context.ServiceRequest
                                                            select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                ServiceRequeststiq = _context.ServiceRequest.Where(d => d.Description.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await ServiceRequeststiq.ToPagedListAsync(pageNumber, pageSize));
        }







        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ServiceRequest == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequest
                .FirstOrDefaultAsync(m => m.ServiceRequestID == id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceRequestID,CreationDate,Title,Description,IsValid,ServiceRequestPicture")] ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ServiceRequest == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequest.FindAsync(id);
            if (serviceRequest == null)
            {
                return NotFound();
            }
            return View(serviceRequest);
        }

        // POST: ServiceRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceRequestID,CreationDate,Title,Description,IsValid,ServiceRequestPicture")] ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.ServiceRequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestExists(serviceRequest.ServiceRequestID))
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
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ServiceRequest == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequest
                .FirstOrDefaultAsync(m => m.ServiceRequestID == id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ServiceRequest == null)
            {
                return Problem("Entity set 'GAPContext.ServiceRequest'  is null.");
            }
            var serviceRequest = await _context.ServiceRequest.FindAsync(id);
            if (serviceRequest != null)
            {
                _context.ServiceRequest.Remove(serviceRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceRequestExists(int id)
        {
          return (_context.ServiceRequest?.Any(e => e.ServiceRequestID == id)).GetValueOrDefault();
        }
    }
}
