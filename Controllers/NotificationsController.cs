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
    public class NotificationsController : Controller
    {
        private readonly GAPContext _context;

        public NotificationsController(GAPContext context)
        {
            _context = context;
        }

        // GET: Notifications
        [HttpGet("/Notifications")]
        [SwaggerOperation(Summary = "Get notifications", Description = "Retrieve a list of notifications.")]
        [SwaggerResponse(200, "List of notifications retrieved successfully.")]
        public async Task<IActionResult> Index()
        {
              return _context.Notification != null ? 
                          View(await _context.Notification.ToListAsync()) :
                          Problem("Entity set 'GAPContext.Notification'  is null.");
        }





        // GET: Notifications/Details/5
        [HttpGet("/Notifications/Details/{id}")]
        [SwaggerOperation(Summary = "Get details of a notification", Description = "Retrieve the details of a notification.")]
        [SwaggerResponse(200, "Notification details retrieved successfully.")]
        [SwaggerResponse(404, "Notification not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Notification == null)
            {
                return NotFound();
            }

            var notification = await _context.Notification
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }





        // GET: Notifications/Create
        [HttpGet("/Notifications/Create")]
        [SwaggerOperation(Summary = "Show notification creation form", Description = "Display the notification creation form.")]
        [SwaggerResponse(200, "Notification creation form displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }






        // POST: Notifications/Create
        [HttpPost("/Notifications/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new notification", Description = "Create a new notification.")]
        [SwaggerResponse(200, "Notification created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Create([Bind("NotificationID,NotificationTitle")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }






        // GET: Notifications/Edit/5
        [HttpGet("/Notifications/Edit/{id}")]
        [SwaggerOperation(Summary = "Show notification edit form", Description = "Display the notification edit form.")]
        [SwaggerResponse(200, "Notification edit form displayed successfully.")]
        [SwaggerResponse(404, "Notification not found.")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Notification == null)
            {
                return NotFound();
            }

            var notification = await _context.Notification.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }






        // POST: Notifications/Edit/5
        [HttpPost("/Notifications/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit a notification", Description = "Edit an existing notification.")]
        [SwaggerResponse(200, "Notification edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Notification not found.")]
        public async Task<IActionResult> Edit(int id, [Bind("NotificationID,NotificationTitle")] Notification notification)
        {
            if (id != notification.NotificationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationExists(notification.NotificationID))
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
            return View(notification);
        }







        // GET: Notifications/Delete/5
        [HttpGet("/Notifications/Delete/{id}")]
        [SwaggerOperation(Summary = "Show notification delete confirmation", Description = "Display the notification delete confirmation.")]
        [SwaggerResponse(200, "Notification delete confirmation displayed successfully.")]
        [SwaggerResponse(404, "Notification not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Notification == null)
            {
                return NotFound();
            }

            var notification = await _context.Notification
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }






        // POST: Notifications/Delete/5
        [HttpPost("/Notifications/Delete/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete a notification", Description = "Delete an existing notification.")]
        [SwaggerResponse(200, "Notification deleted successfully.")]
        [SwaggerResponse(404, "Notification not found.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Notification == null)
            {
                return Problem("Entity set 'GAPContext.Notification'  is null.");
            }
            var notification = await _context.Notification.FindAsync(id);
            if (notification != null)
            {
                _context.Notification.Remove(notification);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }






        /*---------------------------------------------------------------*/






        // Helper: no route
        private bool NotificationExists(int id)
        {
          return (_context.Notification?.Any(e => e.NotificationID == id)).GetValueOrDefault();
        }
    }
}
