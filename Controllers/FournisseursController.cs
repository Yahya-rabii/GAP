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
    public class FournisseursController : Controller
    {
        private readonly GAPContext _context;

        public FournisseursController(GAPContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]

        // GET: Fournisseurs1
        public async Task<IActionResult> Index(int? page)
        {
            IQueryable<Fournisseur> iseriq = from f in _context.Fournisseur
                                             select f;


            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        }


        [Authorize(Roles = "Admin,Fournisseur")]

        // GET: Fournisseurs1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Fournisseur == null)
            {
                return NotFound();
            }

            var fournisseur = await _context.Fournisseur
                .FirstOrDefaultAsync(m => m.FournisseurID == id);
            if (fournisseur == null)
            {
                return NotFound();
            }

            return View(fournisseur);
        }
        [Authorize(Roles = "Admin")]

        // GET: Fournisseurs1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fournisseurs1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create([Bind("FournisseurID,Nom,Email,Password,Adresse,CodePostal,Numtele,NombreTransaction,IsValid")] Fournisseur fournisseur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fournisseur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fournisseur);
        }


        [Authorize(Roles = "Fournisseur")]

        // GET: Fournisseurs1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Fournisseur == null)
            {
                return NotFound();
            }

            var fournisseur = await _context.Fournisseur.FindAsync(id);
            if (fournisseur == null)
            {
                return NotFound();
            }
            return View(fournisseur);
        }




        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Validate(int? id)
        {
            if (id == null || _context.Fournisseur == null)
            {
                return NotFound();
            }

            var fournisseur = await _context.Fournisseur.FindAsync(id);
            if (fournisseur == null)
            {
                return NotFound();
            }
            return View(fournisseur);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Validate(int id)
        {
         

            try
            {
                // Fetch the existing fournisseur from the database
                var existingFournisseur = await _context.Fournisseur.FindAsync(id);

                if (existingFournisseur == null)
                {
                    return NotFound(); // Return 404 Not Found if the fournisseur doesn't exist in the database
                }

                // Set the IsValid property to true and update the fournisseur
                existingFournisseur.IsValid = true;
                _context.Update(existingFournisseur);
                var notification = _context.NotificationAdmin.FirstOrDefault(d => d.FournisseurID == existingFournisseur.FournisseurID);
                if (notification != null)
                {
                    _context.Notification.Remove(notification);
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FournisseurExists(id))
                {
                    return NotFound(); // Return 404 Not Found if the fournisseur doesn't exist in the database
                }
                else
                {
                    throw; // Rethrow the exception if it's not a concurrency issue
                }
            }

            return RedirectToAction("Index", "Fournisseurs");

        }




        // POST: Fournisseurs1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Fournisseur")]

        public async Task<IActionResult> Edit(int id, [Bind("FournisseurID,Nom,Email,Password,Adresse,CodePostal,Numtele,NombreTransaction,IsValid")] Fournisseur fournisseur)
        {
            if (id != fournisseur.FournisseurID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fournisseur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FournisseurExists(fournisseur.FournisseurID))
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
            return View(fournisseur);
        }

        // GET: Fournisseurs1/Delete/5
        [Authorize(Roles = "Admin,Fournisseur")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fournisseur == null)
            {
                return NotFound();
            }

            var fournisseur = await _context.Fournisseur
                .FirstOrDefaultAsync(m => m.FournisseurID == id);
            if (fournisseur == null)
            {
                return NotFound();
            }

            return View(fournisseur);
        }

        // POST: Fournisseurs1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Fournisseur")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fournisseur == null)
            {
                return Problem("Entity set 'GAPContext.Fournisseur'  is null.");
            }
            var fournisseur = await _context.Fournisseur.FindAsync(id);
            if (fournisseur != null)
            {
                _context.Fournisseur.Remove(fournisseur);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FournisseurExists(int id)
        {
          return (_context.Fournisseur?.Any(e => e.FournisseurID == id)).GetValueOrDefault();
        }





        // GET: User/Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Fournisseur fournisseur)
        {
            if (ModelState.IsValid)
            {
                var preregisteredFournisseur = _context.Fournisseur.FirstOrDefault(u => u.Email == fournisseur.Email);

                if (preregisteredFournisseur != null)
                {
                    ModelState.AddModelError("", "User already exists");
                    return RedirectToAction("Register", "Users");
                }
                else
                {
                    fournisseur.Password = HashPassword(fournisseur?.Password);

                    _context.Fournisseur.Add(fournisseur);
                    await _context.SaveChangesAsync();

                    var notificationAdminValidation = new NotificationAdmin
                    {
                        NotificationTitle = "Notification Activation Fournisseur account",
                        FournisseurID = fournisseur.FournisseurID,
                    };

                    _context.NotificationAdmin.Add(notificationAdminValidation);
                    await _context.SaveChangesAsync();

                    // Redirect to the login page after registration
                    return RedirectToAction("Login");
                }
            }
            else
            {
                return RedirectToAction("Register");
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
