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

namespace GAP.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly GAPContext _context;

        public SuppliersController(GAPContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]

        // GET: Suppliers1
        public async Task<IActionResult> Index(int? page)
        {
            IQueryable<Supplier> iseriq = from f in _context.Supplier
                                             select f;


            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        }


        [Authorize(Roles = "Admin,Supplier")]

        // GET: Suppliers1/Details/5
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
        [Authorize(Roles = "Admin")]

        // GET: Suppliers1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

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


        [Authorize(Roles = "Supplier")]

        // GET: Suppliers1/Edit/5
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




        [Authorize(Roles = "Admin")]

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

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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




        // POST: Suppliers1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supplier")]

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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supplier")]

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

        private bool SupplierExists(int id)
        {
          return (_context.Supplier?.Any(e => e.UserID == id)).GetValueOrDefault();
        }





        // GET: User/Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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



        private string HashPassword(string password)
        {
            // use a library like BCrypt or Argon2 to hash the password
            // here's an example using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


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




    }
}
