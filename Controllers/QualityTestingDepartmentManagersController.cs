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
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Controllers
{
    public class QualityTestingDepartmentManagersController : Controller
    {
        private readonly GAPContext _context;

        public QualityTestingDepartmentManagersController(GAPContext context)
        {
            _context = context;
        }



        // GET: QualityTestingDepartmentManagers
        [HttpGet("/QualityTestingDepartmentManagers")]
        [SwaggerOperation(Summary = "Get quality testing department managers", Description = "Retrieve a list of quality testing department managers.")]
        [SwaggerResponse(200, "List of quality testing department managers retrieved successfully.")]
        public async Task<IActionResult> Index(string SearchString, int? page)
        {
            IQueryable<QualityTestingDepartmentManager> iseriq = from s in _context.QualityTestingDepartmentManager
                                                    select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                iseriq = _context.QualityTestingDepartmentManager.Where(s => s.Email.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        }





        // GET: QualityTestingDepartmentManagers/Details/5
        [HttpGet("/QualityTestingDepartmentManagers/Details/{id}")]
        [SwaggerOperation(Summary = "Get details of a quality testing department manager", Description = "Retrieve the details of a quality testing department manager.")]
        [SwaggerResponse(200, "Quality testing department manager details retrieved successfully.")]
        [SwaggerResponse(404, "Quality testing department manager not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.QualityTestingDepartmentManager == null)
            {
                return NotFound();
            }

            var QualityTestingDepartmentManager = await _context.QualityTestingDepartmentManager
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (QualityTestingDepartmentManager == null)
            {
                return NotFound();
            }

            return View(QualityTestingDepartmentManager);
        }





        // GET: QualityTestingDepartmentManagers/Create
        [HttpGet("/QualityTestingDepartmentManagers/Create")]
        [SwaggerOperation(Summary = "Show quality testing department manager creation form", Description = "Display the quality testing department manager creation form.")]
        [SwaggerResponse(200, "Quality testing department manager creation form displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }






        // POST: QualityTestingDepartmentManagers/Create
        [HttpPost("/QualityTestingDepartmentManagers/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new quality testing department manager", Description = "Create a new quality testing department manager with the provided information.")]
        [SwaggerResponse(200, "Quality testing department manager created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] QualityTestingDepartmentManager QualityTestingDepartmentManager)
        {
            if (ModelState.IsValid)
            {

                // Check if the user with the provided email already exists in the database
                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == QualityTestingDepartmentManager.Email);
                if (existingUser != null)
                {
                    // If user with the same email exists, inform the user with a message
                    ModelState.AddModelError("Email", "User with this email already exists.");
                    return View(QualityTestingDepartmentManager);
                }

                QualityTestingDepartmentManager.Password = HashPassword(QualityTestingDepartmentManager?.Password);
                QualityTestingDepartmentManager.Role = UserRole.Quality_testing_department_manager;
                _context.Add(QualityTestingDepartmentManager);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Users");
            }
            return RedirectToAction("Index", "Users");
        }





        // GET: QualityTestingDepartmentManagers/Edit/5
        [HttpGet("/QualityTestingDepartmentManagers/Edit/{id}")]
        [SwaggerOperation(Summary = "Show quality testing department manager edit form", Description = "Display the quality testing department manager edit form.")]
        [SwaggerResponse(200, "Quality testing department manager edit form displayed successfully.")]
        [SwaggerResponse(404, "Quality testing department manager not found.")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.QualityTestingDepartmentManager == null)
            {
                return NotFound();
            }

            var QualityTestingDepartmentManager = await _context.QualityTestingDepartmentManager.FindAsync(id);
            if (QualityTestingDepartmentManager == null)
            {
                return NotFound();
            }
            return View(QualityTestingDepartmentManager);
        }



        // POST: QualityTestingDepartmentManagers/Edit/5
        [HttpPost("/QualityTestingDepartmentManagers/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit a quality testing department manager", Description = "Edit an existing quality testing department manager with the provided information.")]
        [SwaggerResponse(200, "Quality testing department manager edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Quality testing department manager not found.")]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] QualityTestingDepartmentManager QualityTestingDepartmentManager)
    {
        if (id != QualityTestingDepartmentManager.UserID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Check if the user with the provided email already exists in the database
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == QualityTestingDepartmentManager.Email && u.UserID != id);
            if (existingUser != null)
            {
                // If user with the same email exists (and has a different ID), inform the user with a message
                ModelState.AddModelError("Email", "Another user with this email already exists.");
                return View(QualityTestingDepartmentManager);
            }

            try
            {
                _context.Update(QualityTestingDepartmentManager);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QualityTestingDepartmentManagerExists(QualityTestingDepartmentManager.UserID))
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
        return View(QualityTestingDepartmentManager);
    }






        // GET: QualityTestingDepartmentManagers/Delete/5
        [HttpGet("/QualityTestingDepartmentManagers/Delete/{id}")]
        [SwaggerOperation(Summary = "Show quality testing department manager delete confirmation", Description = "Display the quality testing department manager delete confirmation.")]
        [SwaggerResponse(200, "Quality testing department manager delete confirmation displayed successfully.")]
        [SwaggerResponse(404, "Quality testing department manager not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.QualityTestingDepartmentManager == null)
            {
                return NotFound();
            }

            var QualityTestingDepartmentManager = await _context.QualityTestingDepartmentManager
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (QualityTestingDepartmentManager == null)
            {
                return NotFound();
            }

            return View(QualityTestingDepartmentManager);
        }




        // POST: QualityTestingDepartmentManagers/Delete/5
        [HttpPost("/QualityTestingDepartmentManagers/Delete/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete a quality testing department manager", Description = "Delete an existing quality testing department manager.")]
        [SwaggerResponse(200, "Quality testing department manager deleted successfully.")]
        [SwaggerResponse(404, "Quality testing department manager not found.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.QualityTestingDepartmentManager == null)
            {
                return Problem("Entity set 'GAPContext.QualityTestingDepartmentManager'  is null.");
            }
            var QualityTestingDepartmentManager = await _context.QualityTestingDepartmentManager.FindAsync(id);
            if (QualityTestingDepartmentManager != null)
            {
                _context.QualityTestingDepartmentManager.Remove(QualityTestingDepartmentManager);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }






        /*---------------------------------------------------------------*/





        // Helper: no route
        private bool QualityTestingDepartmentManagerExists(int id)
        {
          return (_context.QualityTestingDepartmentManager?.Any(e => e.UserID == id)).GetValueOrDefault();
        }



        // Helper: no route
        private string HashPassword(string password)
        {
            // use a library like BCrypt or Argon2 to hash the password
            // here's an example using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }



    }
}
