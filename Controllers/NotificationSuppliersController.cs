using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Controllers
{
    public class NotificationSuppliersController : Controller
    {
        private readonly GAPContext _context;





        public NotificationSuppliersController(GAPContext context)
        {
            _context = context;
        }







        // GET: NotificationSuppliers
        [HttpGet("/NotificationSuppliers")]
        [SwaggerOperation(Summary = "Get notification suppliers", Description = "Retrieve a list of notification suppliers.")]
        [SwaggerResponse(200, "List of notification suppliers retrieved successfully.")]
        public async Task<IActionResult> Index()
        {
              return _context.NotificationSupplier != null ? 
                          View(await _context.NotificationSupplier.ToListAsync()) :
                          Problem("Entity set 'GAPContext.NotificationSupplier'  is null.");
        }






        // GET: NotificationSuppliers/Details/5
        [HttpGet("/NotificationSuppliers/Details/{id}")]
        [SwaggerOperation(Summary = "Get details of a notification supplier", Description = "Retrieve the details of a notification supplier.")]
        [SwaggerResponse(200, "Notification supplier details retrieved successfully.")]
        [SwaggerResponse(404, "Notification supplier not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NotificationSupplier == null)
            {
                return NotFound();
            }

            var notificationSupplier = await _context.NotificationSupplier
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notificationSupplier == null)
            {
                return NotFound();
            }

            return View(notificationSupplier);
        }







        // GET: NotificationSuppliers/Create
        [HttpGet("/NotificationSuppliers/Create")]
        [SwaggerOperation(Summary = "Show notification supplier creation form", Description = "Display the notification supplier creation form.")]
        [SwaggerResponse(200, "Notification supplier creation form displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }





        // POST: NotificationSuppliers/Create
        [HttpPost("/NotificationSuppliers/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new notification supplier", Description = "Create a new notification supplier.")]
        [SwaggerResponse(200, "Notification supplier created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Create([Bind("SupplierID,SaleOfferID,NotificationID,NotificationTitle")] NotificationSupplier notificationSupplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notificationSupplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notificationSupplier);
        }






        // GET: NotificationSuppliers/Edit/5
        [HttpGet("/NotificationSuppliers/Edit/{id}")]
        [SwaggerOperation(Summary = "Show notification supplier edit form", Description = "Display the notification supplier edit form.")]
        [SwaggerResponse(200, "Notification supplier edit form displayed successfully.")]
        [SwaggerResponse(404, "Notification supplier not found.")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NotificationSupplier == null)
            {
                return NotFound();
            }

            var notificationSupplier = await _context.NotificationSupplier.FindAsync(id);
            if (notificationSupplier == null)
            {
                return NotFound();
            }
            return View(notificationSupplier);
        }







        // POST: NotificationSuppliers/Edit/5
        [HttpPost("/NotificationSuppliers/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit a notification supplier", Description = "Edit an existing notification supplier.")]
        [SwaggerResponse(200, "Notification supplier edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Notification supplier not found.")]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierID,SaleOfferID,NotificationID,NotificationTitle")] NotificationSupplier notificationSupplier)
        {
            if (id != notificationSupplier.NotificationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificationSupplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationSupplierExists(notificationSupplier.NotificationID))
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
            return View(notificationSupplier);
        }







        // GET: NotificationSuppliers/Delete/5
        [HttpGet("/NotificationSuppliers/Delete/{id}")]
        [SwaggerOperation(Summary = "Show notification supplier delete confirmation", Description = "Display the notification supplier delete confirmation.")]
        [SwaggerResponse(200, "Notification supplier delete confirmation displayed successfully.")]
        [SwaggerResponse(404, "Notification supplier not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NotificationSupplier == null)
            {
                return NotFound();
            }

            var notificationSupplier = await _context.NotificationSupplier
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notificationSupplier == null)
            {
                return NotFound();
            }

            return View(notificationSupplier);
        }







        // POST: NotificationSuppliers/Delete/5
        [HttpPost("/NotificationSuppliers/Delete/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete a notification supplier", Description = "Delete an existing notification supplier.")]
        [SwaggerResponse(200, "Notification supplier deleted successfully.")]
        [SwaggerResponse(404, "Notification supplier not found.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NotificationSupplier == null)
            {
                return Problem("Entity set 'GAPContext.NotificationSupplier'  is null.");
            }
            var notificationSupplier = await _context.NotificationSupplier.FindAsync(id);
            if (notificationSupplier != null)
            {
                _context.NotificationSupplier.Remove(notificationSupplier);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        





        /*---------------------------------------------------------------*/






        // Helper: no route
        private bool NotificationSupplierExists(int id)
        {
          return (_context.NotificationSupplier?.Any(e => e.NotificationID == id)).GetValueOrDefault();
        }
    }
}
