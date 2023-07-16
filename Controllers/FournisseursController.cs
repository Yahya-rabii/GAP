using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace GAP.Controllers
{
    public class FournisseursController : Controller
    {
        private readonly GAPContext _context;

        public FournisseursController(GAPContext context)
        {
            _context = context;
        }

        // GET: Fournisseurs
        public async Task<IActionResult> Index()
        {
              return _context.Fournisseur != null ? 
                          View(await _context.Fournisseur.ToListAsync()) :
                          Problem("Entity set 'GAPContext.Fournisseur'  is null.");
        }

        // GET: Fournisseurs/Details/5
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

        // GET: Fournisseurs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fournisseurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FournisseurID,Nom,Email,Password,NombreTransaction,IsValid")] Fournisseur fournisseur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fournisseur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fournisseur);
        }

        // GET: Fournisseurs/Edit/5
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

        // POST: Fournisseurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FournisseurID,Nom,Email,Password,NombreTransaction,IsValid")] Fournisseur fournisseur)
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

        // GET: Fournisseurs/Delete/5
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

        // POST: Fournisseurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Register([Bind("FournisseurID,Nom,Email,Password,NombreTransaction,IsValid")] Fournisseur fournisseur)
        {
            Fournisseur fournisseur1 = fournisseur;


            if (ModelState.IsValid)
            {
                // Hash the password before saving to the database
                // you can use a library like BCrypt or Argon2 to hash the password
                if (fournisseur != null)
                {


                    var preregisteredFournisseur = _context.Fournisseur.FirstOrDefault(u => u.Email == fournisseur.Email);


                    if (preregisteredFournisseur != null)
                    {

                        ModelState.AddModelError("", "user already exist");
                        return RedirectToAction("Register", "Users");

                    }
                    else
                    {


                        fournisseur.Password = HashPassword(fournisseur?.Password);


                        _context.Fournisseur.Add(fournisseur1);
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

            // Return the form with validation errors
            return View(fournisseur);
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
