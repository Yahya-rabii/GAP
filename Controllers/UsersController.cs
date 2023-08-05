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
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
        public async Task<IActionResult> Index(string SearchString, int? page)
        {
            IQueryable<User> iseriq = from s in _context.User
                                      select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                iseriq = _context.User.Where(s => s.Email.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        }


        // GET: Users1/Details/5
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

        // GET: Users1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] User user)
        {
            if (ModelState.IsValid)
            {

                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    // If user with the same email exists (and has a different ID), inform the user with a message
                    ModelState.AddModelError("Email", "Another user with this email already exists.");
                    return View(user);
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users1/Edit/5
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,User updatedUser, IFormFile profilePicture)
        {
            if (id != updatedUser.UserID)
            {
                return NotFound();
            }

        
                try
                {
                    var existingUser = await _context.User.FirstOrDefaultAsync(u => u.UserID == id);

                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    existingUser.Email = updatedUser.Email;
                    existingUser.Password = updatedUser.Password;
                    existingUser.FirstName = updatedUser.FirstName;
                    existingUser.LastName = updatedUser.LastName;

                    if (profilePicture != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await profilePicture.CopyToAsync(memoryStream);
                            existingUser.ProfilePicture = memoryStream.ToArray();
                            existingUser.ProfilePictureFileName = profilePicture.FileName;
                            existingUser.HasCustomProfilePicture = true;
                        }
                    }

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(updatedUser.UserID))
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



        // GET: Users1/Delete/5
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

        // POST: Users1/Delete/5
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







        /*---------------------------------------------------------------*/


        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Login", "Users");
            }

            var registeredUser = await _context.User.FirstOrDefaultAsync(u => u.Email == email);

            if (registeredUser != null && BCrypt.Net.BCrypt.Verify(password, registeredUser.Password))
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, registeredUser.UserID.ToString())
        };

                string redirectAction = string.Empty;
                string redirectController = string.Empty;

                switch (registeredUser)
                {
                    case ReceptServiceAchat _:
                        claims.Add(new Claim(ClaimTypes.Role, "ReceptServiceAchat"));
                        redirectAction = "Index";
                        redirectController = "RapportReceptions";
                        break;
                    case RespServiceAchat _:
                        claims.Add(new Claim(ClaimTypes.Role, "RespServiceAchat"));
                        redirectAction = "Index";
                        redirectController = "DemandeAchats";
                        break;
                    case RespServiceFinance _:
                        claims.Add(new Claim(ClaimTypes.Role, "RespServiceFinance"));
                        redirectAction = "Index";
                        redirectController = "Factures";
                        break;
                    case RespServiceQualite _:
                        claims.Add(new Claim(ClaimTypes.Role, "RespServiceQualite"));
                        redirectAction = "Index";
                        redirectController = "RapportTestQualites";
                        break;
                    case var _ when registeredUser.IsAdmin:
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        redirectAction = "Index";
                        redirectController = "Home";
                        break;
                    default:
                        // User type not recognized
                        ModelState.AddModelError("", "Invalid user type.");
                        return RedirectToAction("Login", "Users");
                }

                return await SignInAndRedirectToAction(claims, redirectAction, redirectController);
            }
            else
            {
                var newregisteredUser = await _context.Fournisseur.FirstOrDefaultAsync(u => u.Email == email);

                if (newregisteredUser != null)
                {
                    if (newregisteredUser.IsValid == true)
                    {
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, newregisteredUser.FournisseurID.ToString()),
                    new Claim(ClaimTypes.Role, "Fournisseur")
                };

                        return await SignInAndRedirectToAction(claims, "IndexFour", "DemandeAchats");
                    }
                    else
                    {
                        // Account is not valid yet
                        ModelState.AddModelError("Email", "Your account is not valid yet. Please wait for approval");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                }

                return RedirectToAction("Login", "Users");
            }
        }

        private async Task<IActionResult> SignInAndRedirectToAction(List<Claim> claims, string actionName, string controllerName)
        {
            var claimsIdentity = new ClaimsIdentity(claims, "Cookie");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToAction(actionName, controllerName);
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
