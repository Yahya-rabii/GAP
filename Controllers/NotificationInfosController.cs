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
    public class NotificationInfosController : Controller
    {
        private readonly GAPContext _context;

        public NotificationInfosController(GAPContext context)
        {
            _context = context;
        }

        // GET: NotificationInfos
        [HttpGet("/NotificationInfos")]
        [SwaggerOperation(Summary = "Get info notifications", Description = "Retrieve a list of info notifications.")]
        [SwaggerResponse(200, "List of info notifications retrieved successfully.")]
        public async Task<IActionResult> Index()
        {
              return _context.NotificationInfo != null ? 
                          View(await _context.NotificationInfo.ToListAsync()) :
                          Problem("Entity set 'GAPContext.NotificationInfo'  is null.");
        }






        // GET: NotificationInfos/Details/5
        [HttpGet("/NotificationInfos/Details/{id}")]
        [SwaggerOperation(Summary = "Get details of an info notification", Description = "Retrieve the details of an info notification.")]
        [SwaggerResponse(200, "Info notification details retrieved successfully.")]
        [SwaggerResponse(404, "Info notification not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NotificationInfo == null)
            {
                return NotFound();
            }

            var NotificationInfo = await _context.NotificationInfo
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (NotificationInfo == null)
            {
                return NotFound();
            }

            return View(NotificationInfo);
        }






        // GET: NotificationInfos/Create
        [HttpGet("/NotificationInfos/Create")]
        [SwaggerOperation(Summary = "Show info notification creation form", Description = "Display the info notification creation form.")]
        [SwaggerResponse(200, "Info notification creation form displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }





        // POST: NotificationInfos/Create
        [HttpPost("/NotificationInfos/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new info notification", Description = "Create a new info notification.")]
        [SwaggerResponse(200, "Info notification created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Create([Bind("UserID,PurchaseQuoteID,NotificationID,NotificationTitle")] NotificationInfo NotificationInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(NotificationInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(NotificationInfo);
        }





        // GET: NotificationInfos/Edit/5
        [HttpGet("/NotificationInfos/Edit/{id}")]
        [SwaggerOperation(Summary = "Show info notification edit form", Description = "Display the info notification edit form.")]
        [SwaggerResponse(200, "Info notification edit form displayed successfully.")]
        [SwaggerResponse(404, "Info notification not found.")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NotificationInfo == null)
            {
                return NotFound();
            }

            var NotificationInfo = await _context.NotificationInfo.FindAsync(id);
            if (NotificationInfo == null)
            {
                return NotFound();
            }
            return View(NotificationInfo);
        }






        // POST: NotificationInfos/Edit/5
        [HttpPost("/NotificationInfos/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit an info notification", Description = "Edit an existing info notification.")]
        [SwaggerResponse(200, "Info notification edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Info notification not found.")]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,PurchaseQuoteID,NotificationID,NotificationTitle")] NotificationInfo NotificationInfo)
        {
            if (id != NotificationInfo.NotificationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(NotificationInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationInfoExists(NotificationInfo.NotificationID))
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
            return View(NotificationInfo);
        }







        // GET: NotificationInfos/Delete/5
        [HttpGet("/NotificationInfos/Delete/{id}")]
        [SwaggerOperation(Summary = "Show info notification delete confirmation", Description = "Display the info notification delete confirmation.")]
        [SwaggerResponse(200, "Info notification delete confirmation displayed successfully.")]
        [SwaggerResponse(404, "Info notification not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NotificationInfo == null)
            {
                return NotFound();
            }

            var NotificationInfo = await _context.NotificationInfo
                .FirstOrDefaultAsync(m => m.NotificationID == id);
            if (NotificationInfo == null)
            {
                return NotFound();
            }

            return View(NotificationInfo);
        }






        // POST: NotificationInfos/Delete/5
        [HttpPost("/NotificationInfos/DeleteConfirmed/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete an info notification", Description = "Delete an existing info notification.")]
        [SwaggerResponse(200, "Info notification deleted successfully.")]
        [SwaggerResponse(404, "Info notification not found.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NotificationInfo == null)
            {
                return Problem("Entity set 'GAPContext.NotificationInfo'  is null.");
            }
            var NotificationInfo = await _context.NotificationInfo.FindAsync(id);
            if (NotificationInfo != null)
            {
                _context.NotificationInfo.Remove(NotificationInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }








        /*---------------------------------------------------------------*/






        // Helper: no route
        private bool NotificationInfoExists(int id)
        {
          return (_context.NotificationInfo?.Any(e => e.NotificationID == id)).GetValueOrDefault();
        }
    }
}
