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
    public class FinanceDepartmentManagersController : Controller
    {
        private readonly GAPContext _context;

        public FinanceDepartmentManagersController(GAPContext context)
        {
            _context = context;
        }



        // GET: FinanceDepartmentManagers
        [HttpGet("/FinanceDepartmentManagers")]
        [SwaggerOperation(Summary = "View list of finance department managers", Description = "Display a list of finance department managers.")]
        [SwaggerResponse(200, "List of finance department managers displayed successfully.")]
        public async Task<IActionResult> Index(string SearchString, int? page)
        {
            IQueryable<FinanceDepartmentManager> iseriq = from s in _context.FinanceDepartmentManager
                                                    select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                iseriq = _context.FinanceDepartmentManager.Where(s => s.Email.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        }





        // GET: FinanceDepartmentManagers/Details/5
        [HttpGet("/FinanceDepartmentManagers/Details/{id}")]
        [SwaggerOperation(Summary = "View finance department manager details", Description = "Display details of a finance department manager.")]
        [SwaggerResponse(200, "Finance department manager details displayed successfully.")]
        [SwaggerResponse(404, "Finance department manager not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FinanceDepartmentManager == null)
            {
                return NotFound();
            }

            var FinanceDepartmentManager = await _context.FinanceDepartmentManager
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (FinanceDepartmentManager == null)
            {
                return NotFound();
            }

            return View(FinanceDepartmentManager);
        }







        // GET: FinanceDepartmentManagers/Create
        [HttpGet("/FinanceDepartmentManagers/Create")]
        [SwaggerOperation(Summary = "Create a new finance department manager", Description = "Display the form to create a new finance department manager.")]
        [SwaggerResponse(200, "Form to create a new finance department manager displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }






        // POST: FinanceDepartmentManagers/Create
        [HttpPost("/FinanceDepartmentManagers/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new finance department manager", Description = "Handle the creation of a new finance department manager.")]
        [SwaggerResponse(200, "Finance department manager created successfully.")]
        [SwaggerResponse(400, "Bad request. User with this email already exists.")]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] FinanceDepartmentManager FinanceDepartmentManager)
        {
            if (ModelState.IsValid)
            {
                // Check if the user with the provided email already exists in the database
                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == FinanceDepartmentManager.Email);
                if (existingUser != null)
                {
                    // If user with the same email exists, inform the user with a message
                    ModelState.AddModelError("Email", "User with this email already exists.");
                    return View(FinanceDepartmentManager);
                }

                FinanceDepartmentManager.Password = HashPassword(FinanceDepartmentManager?.Password);
                FinanceDepartmentManager.Role = UserRole.Finance_department_manager;
                _context.Add(FinanceDepartmentManager);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Users");
            }
            return RedirectToAction("Index", "Users");
        }





        // GET: FinanceDepartmentManagers/Edit/5
        [HttpGet("/FinanceDepartmentManagers/Edit/{id}")]
        [SwaggerOperation(Summary = "Edit a finance department manager", Description = "Display the form to edit a finance department manager.")]
        [SwaggerResponse(200, "Form to edit a finance department manager displayed successfully.")]
        [SwaggerResponse(404, "Finance department manager not found.")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FinanceDepartmentManager == null)
            {
                return NotFound();
            }

            var FinanceDepartmentManager = await _context.FinanceDepartmentManager.FindAsync(id);
            if (FinanceDepartmentManager == null)
            {
                return NotFound();
            }
            return View(FinanceDepartmentManager);
        }






        // POST: FinanceDepartmentManagers/Edit/5
        [HttpPost("/FinanceDepartmentManagers/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit a finance department manager", Description = "Handle the editing of a finance department manager.")]
        [SwaggerResponse(200, "Finance department manager edited successfully.")]
        [SwaggerResponse(400, "Bad request. Another user with this email already exists.")]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] FinanceDepartmentManager FinanceDepartmentManager)
        {
            if (id != FinanceDepartmentManager.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == FinanceDepartmentManager.Email && u.UserID != id);
                if (existingUser != null)
                {
                    // If user with the same email exists (and has a different ID), inform the user with a message
                    ModelState.AddModelError("Email", "Another user with this email already exists.");
                    return View(FinanceDepartmentManager);
                }

                try
                {
                    _context.Update(FinanceDepartmentManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinanceDepartmentManagerExists(FinanceDepartmentManager.UserID))
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
            return View(FinanceDepartmentManager);
        }







        // GET: FinanceDepartmentManagers/Delete/5
        [HttpGet("/FinanceDepartmentManagers/Delete/{id}")]
        [SwaggerOperation(Summary = "Delete a finance department manager", Description = "Display the confirmation form to delete a finance department manager.")]
        [SwaggerResponse(200, "Confirmation form to delete a finance department manager displayed successfully.")]
        [SwaggerResponse(404, "Finance department manager not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FinanceDepartmentManager == null)
            {
                return NotFound();
            }

            var FinanceDepartmentManager = await _context.FinanceDepartmentManager
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (FinanceDepartmentManager == null)
            {
                return NotFound();
            }

            return View(FinanceDepartmentManager);
        }





        // POST: FinanceDepartmentManagers/Delete/5
        [HttpPost("/FinanceDepartmentManagers/Delete/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete a finance department manager", Description = "Handle the deletion of a finance department manager.")]
        [SwaggerResponse(200, "Finance department manager deleted successfully.")]
        [SwaggerResponse(400, "Entity set 'GAPContext.FinanceDepartmentManager' is null.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FinanceDepartmentManager == null)
            {
                return Problem("Entity set 'GAPContext.FinanceDepartmentManager'  is null.");
            }
            var FinanceDepartmentManager = await _context.FinanceDepartmentManager.FindAsync(id);
            if (FinanceDepartmentManager != null)
            {
                _context.FinanceDepartmentManager.Remove(FinanceDepartmentManager);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }






        private bool FinanceDepartmentManagerExists(int id)
        {
          return (_context.FinanceDepartmentManager?.Any(e => e.UserID == id)).GetValueOrDefault();
        }




        private string HashPassword(string password)
        {
            // use a library like BCrypt or Argon2 to hash the password
            // here's an example using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }










        /*---------------------------------------------------------------*/







        // Helper: no route
        public IActionResult Home()
        {
            // Implementation for creating an Offre
            return View();
        }






    }
}
