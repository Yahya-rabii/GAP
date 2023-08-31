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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using X.PagedList;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly GAPContext _context;

        public SuppliersController(GAPContext context)
        {
            _context = context;
        }



        // GET: Suppliers1
        [Authorize(Roles = "Admin")]
        [HttpGet("/Suppliers")]
        [SwaggerOperation(Summary = "Get a list of suppliers", Description = "Retrieve a list of suppliers.")]
        [SwaggerResponse(200, "List of suppliers retrieved successfully.")]
        public async Task<IActionResult> Index(string SearchString, int? page)
        {
            IQueryable<Supplier> iseriq = from f in _context.Supplier
                                             select f;
            if (!string.IsNullOrEmpty(SearchString))
            {
                iseriq = _context.Supplier.Where(s => s.Email.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        }






        // GET: Suppliers1/Details/5
        [Authorize(Roles = "Admin,Supplier")]
        [HttpGet("/Suppliers/Details/{id}")]
        [SwaggerOperation(Summary = "Get supplier details", Description = "Retrieve details of a supplier.")]
        [SwaggerResponse(200, "Supplier details retrieved successfully.")]
        [SwaggerResponse(404, "Supplier not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Supplier == null)
            {
                return NotFound();
            }

            var Supplier = await _context.Supplier
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (Supplier == null)
            {
                return NotFound();
            }

            return View(Supplier);
        }








        // GET: Suppliers1/Create
        [Authorize(Roles = "Admin")]
        [HttpGet("/Suppliers/Create")]
        [SwaggerOperation(Summary = "Show supplier creation form", Description = "Display the supplier creation form.")]
        [SwaggerResponse(200, "Supplier creation form displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }





        // POST: Suppliers1/Create
        [HttpPost("/Suppliers/Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Create a new supplier", Description = "Create a new supplier with the provided information.")]
        [SwaggerResponse(200, "Supplier created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Create([Bind("SupplierID,CompanyName,Email,Password,Adresse,PostalCode,PhoneNumber,TransactionNumber,IsValid")] Supplier Supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Supplier);
        }








        // GET: Suppliers1/Validate/5
        [Authorize(Roles = "Admin")]
        [HttpGet("/Suppliers/Validate/{id}")]
        [SwaggerOperation(Summary = "Show supplier Validation form", Description = "Display the form for Validation a supplier's information.")]
        [SwaggerResponse(200, "Supplier Validation form displayed successfully.")]
        [SwaggerResponse(404, "Supplier not found.")]
        public async Task<IActionResult> Validate(int? id)
        {
            if (id == null || _context.Supplier == null)
            {
                return NotFound();
            }

            var Supplier = await _context.Supplier.FindAsync(id);
            if (Supplier == null)
            {
                return NotFound();
            }
            return View(Supplier);
        }





        // POST: Suppliers1/Validate/5
        [HttpPost("/Suppliers/Validate/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Validate a new supplier", Description = "Validate a new supplier with the provided information.")]
        [SwaggerResponse(200, "Supplier Validated successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Validate(int id)
        {
         

            try
            {
                // Fetch the existing Supplier from the database
                var existingSupplier = await _context.Supplier.FindAsync(id);

                if (existingSupplier == null)
                {
                    return NotFound(); // Return 404 Not Found if the Supplier doesn't exist in the database
                }

                // Set the IsValid property to true and update the Supplier
                existingSupplier.IsValid = true;
                _context.Update(existingSupplier);
                var notification = _context.NotificationAdmin.FirstOrDefault(d => d.SupplierID == existingSupplier.UserID);
                if (notification != null)
                {
                    _context.Notification.Remove(notification);
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
                {
                    return NotFound(); // Return 404 Not Found if the Supplier doesn't exist in the database
                }
                else
                {
                    throw; // Rethrow the exception if it's not a concurrency issue
                }
            }

            return RedirectToAction("Index", "Suppliers");

        }








        // GET: Suppliers1/Edit/5
        [Authorize(Roles = "Supplier,Admin")]
        [HttpGet("/Suppliers/Edit/{id}")]
        [SwaggerOperation(Summary = "Show supplier editing form", Description = "Display the form for editing a supplier's information.")]
        [SwaggerResponse(200, "Supplier editing form displayed successfully.")]
        [SwaggerResponse(404, "Supplier not found.")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Supplier == null)
            {
                return NotFound();
            }

            var Supplier = await _context.Supplier.FindAsync(id);
            if (Supplier == null)
            {
                return NotFound();
            }
            return View(Supplier);
        }








        // POST: Suppliers1/Edit/5
        [HttpPost("/Suppliers/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supplier")]
        [SwaggerOperation(Summary = "Edit supplier information", Description = "Edit the information of a supplier.")]
        [SwaggerResponse(200, "Supplier information edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Supplier not found.")]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierID,CompanyName,Email,Password,Adresse,PostalCode,PhoneNumber,TransactionNumber,IsValid")] Supplier Supplier)
        {
            if (id != Supplier.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(Supplier.UserID))
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
            return View(Supplier);
        }






        // GET: Suppliers1/Delete/5
        [Authorize(Roles = "Admin,Supplier")]
        [HttpGet("/Suppliers/Delete/{id}")]
        [SwaggerOperation(Summary = "Show supplier Deleting form", Description = "Display the form for Deleting a supplier's information.")]
        [SwaggerResponse(200, "Supplier Deleting form displayed successfully.")]
        [SwaggerResponse(404, "Supplier not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Supplier == null)
            {
                return NotFound();
            }

            var Supplier = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (Supplier == null)
            {
                return NotFound();
            }

            return View(Supplier);
        }





        // POST: Suppliers1/Delete/5
        [HttpPost("/Suppliers/Delete/{id}")]
        [Authorize(Roles = "Admin,Supplier")]
        [SwaggerOperation(Summary = "Show supplier Deleting confirmation", Description = "Display the confirmation page for deleting a supplier.")]
        [SwaggerResponse(200, "supplier deletion confirmation page displayed successfully.")]
        [SwaggerResponse(404, "supplier not found.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Supplier == null)
            {
                return Problem("Entity set 'GAPContext.Supplier'  is null.");
            }
            var Supplier = await _context.Supplier.FindAsync(id);
            if (Supplier != null)
            {
                _context.Supplier.Remove(Supplier);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




     



        // GET: User/Register
        [HttpGet("/Suppliers/Register")]
        [SwaggerOperation(Summary = "Show supplier registration form", Description = "Display the supplier registration form.")]
        [SwaggerResponse(200, "Supplier registration form displayed successfully.")]
        public ActionResult Register()
        {
            return View();
        }




        // Post: User/Register
        [HttpPost("/Suppliers/Register")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Register a new supplier", Description = "Register a new supplier with the provided information.")]
        [SwaggerResponse(302, "Supplier registered successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Register(Supplier Supplier)
        {
           
                var preregisteredSupplier = _context.User.FirstOrDefault(u => u.Email == Supplier.Email);

                if (preregisteredSupplier != null)
                {
                    ModelState.AddModelError("", "User already exists");
                    return RedirectToAction("Register", "Users");
                }
                else
                {
                    Supplier.Password = HashPassword(Supplier?.Password);
                    Supplier.Role = UserRole.Supplier;

                _context.Supplier.Add(Supplier);
                    await _context.SaveChangesAsync();

                    var notificationAdminValidation = new NotificationAdmin
                    {
                        NotificationTitle = "Notification Activation Supplier account",
                        SupplierID = Supplier.UserID,
                    };

                    _context.NotificationAdmin.Add(notificationAdminValidation);
                    await _context.SaveChangesAsync();

                    // Redirect to the login page after registration
                    return RedirectToAction("Login");
                }
          
        }






        // Post: Suppliers/Logout
        [HttpGet("/Suppliers/Logout")]
        [SwaggerOperation(Summary = "Supplier logout", Description = "Log out a Supplier.")]
        [SwaggerResponse(302, "Supplier logged out and redirected to login page.")]
        public async Task<IActionResult> Logout()
        {

            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear the user's cookies
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                IsPersistent = false, // or true if you want to keep the user's cookies
                ExpiresUtc = DateTime.UtcNow.AddMinutes(-1)
            });

            // Redirect to the login page
            return RedirectToAction("Login", "Users");

        }





        /*---------------------------------------------------------------*/





        // Helper: no route
        private string HashPassword(string password)
        {
            // use a library like BCrypt or Argon2 to hash the password
            // here's an example using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }




        // Helper: no route
        private bool SupplierExists(int id)
        {
            return (_context.Supplier?.Any(e => e.UserID == id)).GetValueOrDefault();
        }


    }
}
