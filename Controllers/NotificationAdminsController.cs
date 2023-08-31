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
    public class NotificationAdminsController : Controller
    {
        private readonly GAPContext _context;

        public NotificationAdminsController(GAPContext context)
        {
            _context = context;
        }




        // GET: NotificationAdmins
        [HttpGet("/NotificationAdmins")]
        [SwaggerOperation(Summary = "Get admin notifications", Description = "Retrieve a list of admin notifications.")]
        [SwaggerResponse(200, "List of admin notifications retrieved successfully.")]
        public async Task<IActionResult> Index()
        {
              return _context.NotificationAdmin != null ? 
                          View(await _context.NotificationAdmin.ToListAsync()) :
                          Problem("Entity set 'GAPContext.NotificationAdmin'  is null.");
        }




        // GET: NotificationAdmins/Details/5
        [HttpGet("/NotificationAdmins/Details/{id}")]
        [SwaggerOperation(Summary = "Get details of an admin notification", Description = "Retrieve the details of an admin notification.")]
        [SwaggerResponse(200, "Admin notification details retrieved successfully.")]
        [SwaggerResponse(404, "Admin notification not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NotificationAdmin == null)
            {
                return NotFound();
            }

            var notificationAdmin = await _context.NotificationAdmin
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notificationAdmin == null)
            {
                return NotFound();
            }

            return View(notificationAdmin);
        }





        // GET: NotificationAdmins/Create
        [HttpGet("/NotificationAdmins/Create")]
        [SwaggerOperation(Summary = "Show admin notification creation form", Description = "Display the admin notification creation form.")]
        [SwaggerResponse(200, "Admin notification creation form displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }




        // POST: NotificationAdmins/Create
        [HttpPost("/NotificationAdmins/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new admin notification", Description = "Create a new admin notification.")]
        [SwaggerResponse(200, "Admin notification created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Create([Bind("SupplierID,NotificationID,NotificationTitle")] NotificationAdmin notificationAdmin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notificationAdmin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notificationAdmin);
        }





        // GET: NotificationAdmins/Edit/5
        [HttpGet("/NotificationAdmins/Edit/{id}")]
        [SwaggerOperation(Summary = "Show admin notification edit form", Description = "Display the admin notification edit form.")]
        [SwaggerResponse(200, "Admin notification edit form displayed successfully.")]
        [SwaggerResponse(404, "Admin notification not found.")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NotificationAdmin == null)
            {
                return NotFound();
            }

            var notificationAdmin = await _context.NotificationAdmin.FindAsync(id);
            if (notificationAdmin == null)
            {
                return NotFound();
            }
            return View(notificationAdmin);
        }







        // POST: NotificationAdmins/Edit/5
        [HttpPost("/NotificationAdmins/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit an admin notification", Description = "Edit an existing admin notification.")]
        [SwaggerResponse(200, "Admin notification edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Admin notification not found.")]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierID,NotificationID,NotificationTitle")] NotificationAdmin notificationAdmin)
        {
            if (id != notificationAdmin.NotificationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificationAdmin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationAdminExists(notificationAdmin.NotificationID))
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
            return View(notificationAdmin);
        }






        // GET: NotificationAdmins/Delete/5
        [HttpGet("/NotificationAdmins/Delete/{id}")]
        [SwaggerOperation(Summary = "Show admin notification delete confirmation", Description = "Display the admin notification delete confirmation.")]
        [SwaggerResponse(200, "Admin notification delete confirmation displayed successfully.")]
        [SwaggerResponse(404, "Admin notification not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NotificationAdmin == null)
            {
                return NotFound();
            }

            var notificationAdmin = await _context.NotificationAdmin
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notificationAdmin == null)
            {
                return NotFound();
            }

            return View(notificationAdmin);
        }







        // POST: NotificationAdmins/Delete/5
        [HttpPost("/NotificationAdmins/DeleteConfirmed/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete an admin notification", Description = "Delete an existing admin notification.")]
        [SwaggerResponse(200, "Admin notification deleted successfully.")]
        [SwaggerResponse(404, "Admin notification not found.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NotificationAdmin == null)
            {
                return Problem("Entity set 'GAPContext.NotificationAdmin'  is null.");
            }
            var notificationAdmin = await _context.NotificationAdmin.FindAsync(id);
            if (notificationAdmin != null)
            {
                _context.NotificationAdmin.Remove(notificationAdmin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }








        /*---------------------------------------------------------------*/







        // Helper: no route
        private bool NotificationAdminExists(int id)
        {
          return (_context.NotificationAdmin?.Any(e => e.NotificationID == id)).GetValueOrDefault();
        }
    }
}
