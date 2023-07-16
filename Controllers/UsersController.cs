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
using GAP.Helper;

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
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,UserType")] User user)
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
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,FirstName,LastName,UserType")] User user)
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

    

        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var userType = GetUserTitulairFromTable(email);

                if (!string.IsNullOrEmpty(userType))
                {
                    var user = GetUserFromTable(userType, email, password);

                    if (user != null)
                    {
                        var claims = new Claim[]
                        {
                    new Claim(ClaimTypes.NameIdentifier, GetUserId(user).ToString()),
                    new Claim(ClaimTypes.Role, userType)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, "Cookie");
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return GetRedirectActionForUserType(userType);
                    }
                }

                ModelState.AddModelError("", "Invalid email or password.");
            }

            return RedirectToAction("Login", "Users");
        }

        private string GetUserTitulairFromTable(string email)
        {
            if (_context.Admin.Any(u => u.Email == email))
                return "Admin";
            if (_context.RespServiceAchat.Any(u => u.Email == email))
                return "RespServiceAchat";
            if (_context.ReceptServiceAchat.Any(u => u.Email == email))
                return "ReceptServiceAchat";
            if (_context.RespServiceFinance.Any(u => u.Email == email))
                return "RespServiceFinance";
            if (_context.RespServiceQualite.Any(u => u.Email == email))
                return "RespServiceQualite";

            return null;
        }

        private object GetUserFromTable(string userType, string email, string password)
        {
            object user = null;

            switch (userType)
            {
                case "Admin":
                    user = GetAdminFromTable(email);
                    break;
                case "RespServiceAchat":
                    user = GetRespServiceAchatFromTable(email);
                    break;
                case "ReceptServiceAchat":
                    user = GetReceptServiceAchatFromTable(email);
                    break;
                case "RespServiceFinance":
                    user = GetRespServiceFinanceFromTable(email);
                    break;
                case "RespServiceQualite":
                    user = GetRespServiceQualiteFromTable(email);
                    break;
            }

            if (user != null && VerifyUserPassword(user, userType, password))
                return user;

            return null;
        }

        private bool VerifyUserPassword(object user, string userType, string password)
        {
            switch (userType)
            {
                case "Admin":
                    var admin = (Admin)user;
                    return BCrypt.Net.BCrypt.Verify(password, admin.Password);

                case "RespServiceAchat":
                    var respServiceAchat = (RespServiceAchat)user;
                    return BCrypt.Net.BCrypt.Verify(password, respServiceAchat.Password);

                case "ReceptServiceAchat":
                    var receptServiceAchat = (ReceptServiceAchat)user;
                    return BCrypt.Net.BCrypt.Verify(password, receptServiceAchat.Password);

                case "RespServiceFinance":
                    var respServiceFinance = (RespServiceFinance)user;
                    return BCrypt.Net.BCrypt.Verify(password, respServiceFinance.Password);

                case "RespServiceQualite":
                    var respServiceQualite = (RespServiceQualite)user;
                    return BCrypt.Net.BCrypt.Verify(password, respServiceQualite.Password);

                default:
                    return false;
            }
        }


        private int GetUserId(object user)
        {
            switch (user)
            {
                case Admin admin:
                    return admin.AdminID;
                case RespServiceAchat respServiceAchat:
                    return respServiceAchat.RespServiceAchatID;
                case ReceptServiceAchat receptServiceAchat:
                    return receptServiceAchat.ReceptServiceAchatID;
                case RespServiceFinance respServiceFinance:
                    return respServiceFinance.RespServiceFinanceID;
                case RespServiceQualite respServiceQualite:
                    return respServiceQualite.RespServiceQualiteID;
                default:
                    return 0;
            }
        }

        private Admin GetAdminFromTable(string email)
        {
            return _context.Admin.FirstOrDefault(u => u.Email == email);
        }

        private RespServiceAchat GetRespServiceAchatFromTable(string email)
        {
            return _context.RespServiceAchat.FirstOrDefault(u => u.Email == email);
        }

        private ReceptServiceAchat GetReceptServiceAchatFromTable(string email)
        {
            return _context.ReceptServiceAchat.FirstOrDefault(u => u.Email == email);
        }

        private RespServiceFinance GetRespServiceFinanceFromTable(string email)
        {
            return _context.RespServiceFinance.FirstOrDefault(u => u.Email == email);
        }

        private RespServiceQualite GetRespServiceQualiteFromTable(string email)
        {
            return _context.RespServiceQualite.FirstOrDefault(u => u.Email == email);
        }

        private IActionResult GetRedirectActionForUserType(string userType)
        {
            switch (userType)
            {
                case "Admin":
                    return RedirectToAction("Index", "Home");
                case "RespServiceAchat":
                    return RedirectToAction("Index", "DemandeAchats");
                case "ReceptServiceAchat":
                    return RedirectToAction("Index", "RapportReceptions");
                case "RespServiceFinance":
                    return RedirectToAction("Index", "Factures");
                case "RespServiceQualite":
                    return RedirectToAction("Index", "RapportTestQualites");
                default:
                    return RedirectToAction("Login", "Users");
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
