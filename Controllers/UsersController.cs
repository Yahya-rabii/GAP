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
using System.Security.Claims;

namespace GAP.Controllers
{
    public class UsersController : Controller
    {
        private readonly GAPContext _context;

        public UsersController(GAPContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
              return _context.User != null ? 
                          View(await _context.User.ToListAsync()) :
                          Problem("Entity set 'GAPContext.User'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'GAPContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.UserID == id)).GetValueOrDefault();
        }



        /////////////////////////////////////////////////////////////////////




        // GET: User/Register
        public ActionResult Register()
        {
            return View();
        }




        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] User user)
        {
            if (ModelState.IsValid)
            {

                // Hash the password before saving to the database
                // you can use a library like BCrypt or Argon2 to hash the password
                if (user != null)
                {


                    var preregisteredUser = _context.User.FirstOrDefault(u => u.Email == user.Email);


                    if (preregisteredUser != null)
                    {

                        ModelState.AddModelError("", "user already exist");
                        return RedirectToAction("Register", "Users");

                    }
                    else
                    {

                        user.Password = HashPassword(user?.Password);

                        _context.Add(user);
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
            else
            {
                // Return the form with validation errors
                return View(user);
            }
        }



        /*---------------------------------------------------------------*/


        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {


            // Hash the password before comparing it to the database



            if (email != null && password != null)
            {
                var registeredUser = _context.User.FirstOrDefault(u => u.Email == email);
                var x = false;

                if (registeredUser != null) x = BCrypt.Net.BCrypt.Verify(password, registeredUser?.Password);

                var claims = new List<Claim>();
                var claimsIdentity = new ClaimsIdentity();

                if (registeredUser != null && x)
                {

                    if (registeredUser.IsAdmin)
                    {

                        claims = new List<Claim> { new Claim(ClaimTypes.Role, "admin"), new Claim(ClaimTypes.NameIdentifier, registeredUser.UserID.ToString()) };

                        claimsIdentity = new ClaimsIdentity(claims, "Cookie");

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Index", "Home");


                    }
                    else
                    {

                        // Log the user in
                        // you can create a session or a cookie here to keep the user logged in

                        claims = new List<Claim> { new Claim(ClaimTypes.Role, "user"), new Claim(ClaimTypes.NameIdentifier, registeredUser.UserID.ToString()) };

                        claimsIdentity = new ClaimsIdentity(claims, "Cookie");

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        // Redirect to the home page
                        return RedirectToAction("List", "Produits");

                    }


                }

                else
                {
                    // Invalid credentials
                    ModelState.AddModelError("", "Invalid email or password.");
                    return RedirectToAction("Login", "Users");

                }

            }

            else return RedirectToAction("Login", "Users");

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



















        /////////////////////////////////////////////////////////////////////









    }
}
