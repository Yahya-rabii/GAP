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
    public class NotificationReclamationsController : Controller
    {
        private readonly GAPContext _context;

        public NotificationReclamationsController(GAPContext context)
        {
            _context = context;
        }




        // GET: NotificationReclamations
        [HttpGet("/NotificationReclamations")]
        [SwaggerOperation(Summary = "Get reclamation notifications", Description = "Retrieve a list of reclamation notifications.")]
        [SwaggerResponse(200, "List of reclamation notifications retrieved successfully.")]
        public async Task<IActionResult> Index()
        {
              return _context.NotificationReclamation != null ? 
                          View(await _context.NotificationReclamation.ToListAsync()) :
                          Problem("Entity set 'GAPContext.NotificationReclamation'  is null.");
        }





        // GET: NotificationReclamations/Details/5
        [HttpGet("/NotificationReclamations/Details/{id}")]
        [SwaggerOperation(Summary = "Get details of a reclamation notification", Description = "Retrieve the details of a reclamation notification.")]
        [SwaggerResponse(200, "Reclamation notification details retrieved successfully.")]
        [SwaggerResponse(404, "Reclamation notification not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NotificationReclamation == null)
            {
                return NotFound();
            }

            var notificationReclamation = await _context.NotificationReclamation
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notificationReclamation == null)
            {
                return NotFound();
            }

            return View(notificationReclamation);
        }







        // GET: NotificationReclamations/Create
        [HttpGet("/NotificationReclamations/Create")]
        [SwaggerOperation(Summary = "Show reclamation notification creation form", Description = "Display the reclamation notification creation form.")]
        [SwaggerResponse(200, "Reclamation notification creation form displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }





        // POST: NotificationReclamations/Create
        [HttpPost("/NotificationReclamations/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new reclamation notification", Description = "Create a new reclamation notification.")]
        [SwaggerResponse(200, "Reclamation notification created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Create([Bind("NotificationID,NotificationTitle")] NotificationReclamation notificationReclamation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notificationReclamation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notificationReclamation);
        }




        // GET: NotificationReclamations/Edit/5
        [HttpGet("/NotificationReclamations/Edit/{id}")]
        [SwaggerOperation(Summary = "Show reclamation notification edit form", Description = "Display the reclamation notification edit form.")]
        [SwaggerResponse(200, "Reclamation notification edit form displayed successfully.")]
        [SwaggerResponse(404, "Reclamation notification not found.")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NotificationReclamation == null)
            {
                return NotFound();
            }

            var notificationReclamation = await _context.NotificationReclamation.FindAsync(id);
            if (notificationReclamation == null)
            {
                return NotFound();
            }
            return View(notificationReclamation);
        }





        // POST: NotificationReclamations/Edit/5
        [HttpPost("/NotificationReclamations/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit a reclamation notification", Description = "Edit an existing reclamation notification.")]
        [SwaggerResponse(200, "Reclamation notification edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Reclamation notification not found.")]
        public async Task<IActionResult> Edit(int id, [Bind("NotificationID,NotificationTitle")] NotificationReclamation notificationReclamation)
        {
            if (id != notificationReclamation.NotificationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificationReclamation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationReclamationExists(notificationReclamation.NotificationID))
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
            return View(notificationReclamation);
        }






        // GET: NotificationReclamations/Delete/5
        [HttpGet("/NotificationReclamations/Delete/{id}")]
        [SwaggerOperation(Summary = "Show reclamation notification delete confirmation", Description = "Display the reclamation notification delete confirmation.")]
        [SwaggerResponse(200, "Reclamation notification delete confirmation displayed successfully.")]
        [SwaggerResponse(404, "Reclamation notification not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NotificationReclamation == null)
            {
                return NotFound();
            }

            var notificationReclamation = await _context.NotificationReclamation
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (notificationReclamation == null)
            {
                return NotFound();
            }

            return View(notificationReclamation);
        }









        // POST: NotificationReclamations/Delete/5
        [HttpPost("/NotificationReclamations/Delete/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete a reclamation notification", Description = "Delete an existing reclamation notification.")]
        [SwaggerResponse(200, "Reclamation notification deleted successfully.")]
        [SwaggerResponse(404, "Reclamation notification not found.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NotificationReclamation == null)
            {
                return Problem("Entity set 'GAPContext.NotificationReclamation'  is null.");
            }
            var notificationReclamation = await _context.NotificationReclamation.FindAsync(id);
            if (notificationReclamation != null)
            {
                _context.NotificationReclamation.Remove(notificationReclamation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }








        /*---------------------------------------------------------------*/






        // Helper: no route
        private bool NotificationReclamationExists(int id)
        {
          return (_context.NotificationReclamation?.Any(e => e.NotificationID == id)).GetValueOrDefault();
        }
    }
}
