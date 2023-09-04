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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using OfficeOpenXml;
using Humanizer;
using iTextSharp.text.pdf.qrcode;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Swashbuckle.AspNetCore.Annotations;

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
        [HttpGet("/Users")]
        [SwaggerOperation(Summary = "Get a list of users", Description = "Retrieve a list of users.")]
        [SwaggerResponse(200, "List of users retrieved successfully.")]
        public async Task<IActionResult> Index(string SearchString, int? page)
        {
            IQueryable<User> iseriq = from s in _context.User.Where(u => u.Role != UserRole.Supplier)
                                      select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                iseriq = _context.User.Where(s => s.Email.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        }




        // Post: ImportFromExcel
        [HttpPost("/Users/ImportFromExcel")]
        [SwaggerOperation(Summary = "Import users from Excel", Description = "Import users from an Excel file.")]
        [SwaggerResponse(200, "Users imported successfully.")]
        public async Task<IActionResult> ImportFromExcel(IFormFile excelFile)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Or LicenseContext.Commercial

            if (excelFile == null || excelFile.Length <= 0)
            {
                // Handle error: No file uploaded
                return RedirectToAction("Index");
            }

            string[] duplicateEmails = new string[0]; // Initialize as an empty array

            using (var package = new ExcelPackage(excelFile.OpenReadStream()))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string email = worksheet.Cells[row, 1].Value?.ToString();

                    if (string.IsNullOrEmpty(email))
                    {
                        // Skip empty rows
                        continue;
                    }

                    if (_context.User.Any(u => u.Email == email))
                    {
                        Array.Resize(ref duplicateEmails, duplicateEmails.Length + 1);
                        duplicateEmails[duplicateEmails.Length - 1] = email;
                        continue;
                    }

                    string firstName = worksheet.Cells[row, 2].Value?.ToString();
                    string lastName = worksheet.Cells[row, 3].Value?.ToString();
                    string password = HashPassword(worksheet.Cells[row, 4].Value?.ToString());
                    string roleString = worksheet.Cells[row, 5].Value?.ToString();

                    if (Enum.TryParse<UserRole>(roleString, out var userRole))
                    {
                        var newUser = new User
                        {
                            Email = email,
                            FirstName = firstName,
                            LastName = lastName,
                            Password = password,
                            Role = userRole,
                        };
                        _context.User.Add(newUser);
                    }
                }

                await _context.SaveChangesAsync();
            }

            if (duplicateEmails.Any())
            {
                TempData["DuplicateEmails"] = duplicateEmails;
            }

            return RedirectToAction("Index");
        }




        // GET: Users1/Details/5
        [HttpGet("/Users/Details/{id}")]
        [SwaggerOperation(Summary = "Get user details", Description = "Retrieve details of a user.")]
        [SwaggerResponse(200, "User details retrieved successfully.")]
        [SwaggerResponse(404, "User not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);

            if (user == null)
            {
                return NotFound();
            }

            // Get the user's profile picture (assuming you have a property for it in the User model)
            var userProfilePicture = user.ProfilePicture;



            // Pass the user, profile picture, and claims to the view
            ViewBag.UserProfilePicture = userProfilePicture;

            return View(user);
        }




        // GET: User/SupplierProfile
        [HttpGet("/Users/Profile")]
        [SwaggerOperation(Summary = "Show user deletion confirmation", Description = "Display the user Profile page for login a user.")]
        [SwaggerResponse(200, "user Profile page displayed successfully.")]
        [SwaggerResponse(404, "user not found.")]
        public async Task<IActionResult> Profile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);

            if (user == null)
            {
                return NotFound();
            }

            // Get the user's profile picture (assuming you have a property for it in the User model)
            var userProfilePicture = user.ProfilePicture;

            // Get the current user's role claims
            var currentUserRoles = User.FindAll(ClaimTypes.Role).Select(x => x.Value);

            // Pass the user, profile picture, and role claims to the view
            ViewBag.UserProfilePicture = userProfilePicture;
            ViewBag.CurrentUserRoles = currentUserRoles;

            return View(user);
        }




        // GET: User/SupplierProfile
        [HttpGet("/Users/SupplierProfile")]
        [SwaggerOperation(Summary = "Show Supplier deletion confirmation", Description = "Display the Supplier Profile page for login a user.")]
        [SwaggerResponse(200, "Supplier Profile page displayed successfully.")]
        [SwaggerResponse(404, "Supplier not found.")]
        public async Task<IActionResult> SupplierProfile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);

            if (user == null)
            {
                return NotFound();
            }

            // Get the user's profile picture (assuming you have a property for it in the User model)
            var userProfilePicture = user.ProfilePicture;

            // Get the current user's role claims
            var currentUserRoles = User.FindAll(ClaimTypes.Role).Select(x => x.Value);

            // Pass the user, profile picture, and role claims to the view
            ViewBag.UserProfilePicture = userProfilePicture;
            ViewBag.CurrentUserRoles = currentUserRoles;

            return View(user);
        }




        // GET: Users1/Create
        [HttpGet("/Users/Create")]
        [SwaggerOperation(Summary = "Show user creation form", Description = "Display the user creation form.")]
        [SwaggerResponse(200, "User creation form displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }




        // POST: Users1/Create
        [HttpPost("/Users/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new user", Description = "Create a new user with the provided information.")]
        [SwaggerResponse(200, "User created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
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
        [HttpGet("/Users/Edit/{id}")]
        [SwaggerOperation(Summary = "Show user editing form", Description = "Display the form for editing a user's information.")]
        [SwaggerResponse(200, "User editing form displayed successfully.")]
        [SwaggerResponse(404, "User not found.")]
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
            // Get the user's profile picture (assuming you have a property for it in the User model)
            var userProfilePicture = user.ProfilePicture;

            // Get the current user's role claims
            var currentUserRoles = User.FindAll(ClaimTypes.Role).Select(x => x.Value);

            // Pass the user, profile picture, and role claims to the view
            ViewBag.UserProfilePicture = userProfilePicture;
            ViewBag.CurrentUserRoles = currentUserRoles;

            return View(user);
        }




        [HttpPost("/Users/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Update user information", Description = "Update a user's information with the provided data.")]
        [SwaggerResponse(200, "User information updated successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "User not found.")]
        public async Task<IActionResult> Edit(int id, User updatedUser, IFormFile profilePicture)
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
                existingUser.Password = HashPassword(updatedUser.Password);
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
            return RedirectToAction("Details", "Users", new { id = id });

        }




        // GET: Users1/ProfileEdit/5
        [HttpGet("/Users/ProfileEdit/{id}")]
        [SwaggerOperation(Summary = "Show user profile editing form", Description = "Display the form for editing a user's own profile.")]
        [SwaggerResponse(200, "User profile editing form displayed successfully.")]
        [SwaggerResponse(404, "User not found.")]
        public async Task<IActionResult> ProfileEdit(int? id)
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
            // Get the user's profile picture (assuming you have a property for it in the User model)
            var userProfilePicture = user.ProfilePicture;

            // Get the current user's role claims
            var currentUserRoles = User.FindAll(ClaimTypes.Role).Select(x => x.Value);

            // Pass the user, profile picture, and role claims to the view
            ViewBag.UserProfilePicture = userProfilePicture;
            ViewBag.CurrentUserRoles = currentUserRoles;

            return View(user);
        }




        // POST: Users1/ProfileEdit/5
        [HttpPost("/Users/ProfileEdit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Update user profile", Description = "Update the user's own profile information.")]
        [SwaggerResponse(200, "User profile updated successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "User not found.")]
        public async Task<IActionResult> ProfileEdit(int id, User updatedUser, IFormFile profilePicture)
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

            return RedirectToAction("Profile", "Users", new { id = id });
        }





        // GET: Users1/SupplierProfileEdit/5
        [HttpGet("/Users/SupplierProfileEdit/{id}")]
        [SwaggerOperation(Summary = "Show supplier profile editing form", Description = "Display the form for editing a supplier's profile.")]
        [SwaggerResponse(200, "Supplier profile editing form displayed successfully.")]
        [SwaggerResponse(404, "Supplier not found.")]
        public async Task<IActionResult> SupplierProfileEdit(int? id)
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
            // Get the user's profile picture (assuming you have a property for it in the User model)
            var userProfilePicture = user.ProfilePicture;

            // Get the current user's role claims
            var currentUserRoles = User.FindAll(ClaimTypes.Role).Select(x => x.Value);

            // Pass the user, profile picture, and role claims to the view
            ViewBag.UserProfilePicture = userProfilePicture;
            ViewBag.CurrentUserRoles = currentUserRoles;

            return View(user);
        }




        // POST: Users1/SupplierProfileEdit/5
        [HttpPost("/Users/SupplierProfileEdit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Update supplier profile", Description = "Update a supplier's profile information.")]
        [SwaggerResponse(200, "Supplier profile updated successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Supplier not found.")]
        public async Task<IActionResult> SupplierProfileEdit(int id, Supplier updatedUser, IFormFile profilePicture)
        {
            if (id != updatedUser.UserID)
            {
                return NotFound();
            }

            try
            {
                var existingUser = await _context.Supplier.FirstOrDefaultAsync(u => u.UserID == id);

                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.Password = updatedUser.Password;
                existingUser.Email = updatedUser.Email;
                existingUser.CompanyName = updatedUser.CompanyName;

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

            return RedirectToAction("SupplierProfile", "Users", new { id = id });
        }




        // GET: Users1/Delete/5
        [HttpGet("/Users/Delete/{id}")]
        [SwaggerOperation(Summary = "Show user deletion confirmation", Description = "Display the confirmation page for deleting a user.")]
        [SwaggerResponse(200, "User deletion confirmation page displayed successfully.")]
        [SwaggerResponse(404, "User not found.")]
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
        [HttpPost("/Users/Delete/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete a user", Description = "Delete the specified user.")]
        [SwaggerResponse(200, "User deleted successfully.")]
        [SwaggerResponse(404, "User not found.")]
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







        // GET: User/Login
        [HttpGet("/Users/Login")]
        [SwaggerOperation(Summary = "Show user deletion confirmation", Description = "Display the confirmation page for login a user.")]
        [SwaggerResponse(200, "User login confirmation page displayed successfully.")]
        [SwaggerResponse(404, "User not found.")]
        public ActionResult Login()
        {
            return View();
        }




        // POST: User/Login
        [HttpPost("/Users/Login")]
        [SwaggerOperation(Summary = "User login", Description = "Authenticate a user.")]
        [SwaggerResponse(200, "User authenticated successfully.")]
        [SwaggerResponse(302, "Redirect to appropriate action after login.")]
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
                    case PurchasingReceptionist _:
                        claims.Add(new Claim(ClaimTypes.Role, "PurchasingReceptionist"));
                        registeredUser.Role = UserRole.Purchasing_receptionist;
                        redirectAction = "Index";
                        redirectController = "ReceptionReports";
                        break;
                    case PurchasingDepartmentManager _:
                        claims.Add(new Claim(ClaimTypes.Role, "PurchasingDepartmentManager"));
                        registeredUser.Role = UserRole.Purchasing_department_manager;
                        redirectAction = "Index";
                        redirectController = "PurchaseRequests";
                        break;
                    case FinanceDepartmentManager _:
                        claims.Add(new Claim(ClaimTypes.Role, "FinanceDepartmentManager"));
                        registeredUser.Role = UserRole.Finance_department_manager;
                        redirectAction = "Index";
                        redirectController = "Bills";
                        break;
                    case QualityTestingDepartmentManager _:
                        claims.Add(new Claim(ClaimTypes.Role, "QualityTestingDepartmentManager"));
                        registeredUser.Role = UserRole.Quality_testing_department_manager;
                        redirectAction = "Index";
                        redirectController = "QualityTestReports";
                        break;
                    case ProjectManager _:
                        claims.Add(new Claim(ClaimTypes.Role, "ProjectManager"));
                        registeredUser.Role = UserRole.Project_Manager;
                        redirectAction = "Index";
                        redirectController = "Projects";
                        break;
                    case Supplier _:
                        if (((Supplier)registeredUser).IsValid == true)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "Supplier"));
                            registeredUser.Role = UserRole.Supplier;
                            redirectAction = "IndexFour";
                            redirectController = "PurchaseRequests";
                        }
                        else
                        {
                            ModelState.AddModelError("Email", "Your account is not valid yet. Please wait for approval");

                        }
                        break;
                    case var _ when registeredUser.IsAdmin:
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        registeredUser.Role = UserRole.Admin;
                        redirectAction = "Index";
                        redirectController = "Home";
                        break;
                    default:
                        // User type not recognized
                        ModelState.AddModelError("", "Invalid user type.");
                        return RedirectToAction("Login", "Users");
                }
                _context.User.Update(registeredUser);
                _context.SaveChanges();
                return await SignInAndRedirectToAction(claims, redirectAction, redirectController);
            }
            else
            {
                return RedirectToAction("Login", "Users");

            }

        }












        [HttpGet("/Users/Logout")]
        [SwaggerOperation(Summary = "User logout", Description = "Log out a user.")]
        [SwaggerResponse(302, "User logged out and redirected to login page.")]
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
        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.UserID == id)).GetValueOrDefault();
        }




        // Helper: no route
        private async Task<IActionResult> SignInAndRedirectToAction(List<Claim> claims, string actionName, string controllerName)
        {
            var claimsIdentity = new ClaimsIdentity(claims, "Cookie");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToAction(actionName, controllerName);
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