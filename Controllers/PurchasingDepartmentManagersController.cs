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

namespace GAP.Controllers
{
    public class PurchasingDepartmentManagersController : Controller
    {
        private readonly GAPContext _context;

        public PurchasingDepartmentManagersController(GAPContext context)
        {
            _context = context;
        }

        // GET: PurchasingDepartmentManagers
        public async Task<IActionResult> Index(string SearchString, int? page)
        {
            IQueryable<PurchasingDepartmentManager> iseriq = from s in _context.PurchasingDepartmentManager
                                      select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                iseriq = _context.PurchasingDepartmentManager.Where(s => s.Email.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: PurchasingDepartmentManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PurchasingDepartmentManager == null)
            {
                return NotFound();
            }

            var PurchasingDepartmentManager = await _context.PurchasingDepartmentManager
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (PurchasingDepartmentManager == null)
            {
                return NotFound();
            }

            return View(PurchasingDepartmentManager);

        }

        // GET: PurchasingDepartmentManagers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PurchasingDepartmentManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] PurchasingDepartmentManager PurchasingDepartmentManager)
        {
            if (ModelState.IsValid)
            {

                // Check if the user with the provided email already exists in the database
                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == PurchasingDepartmentManager.Email);
                if (existingUser != null)
                {
                    // If user with the same email exists, inform the user with a message
                    ModelState.AddModelError("Email", "User with this email already exists.");
                    return View(PurchasingDepartmentManager);
                }
                PurchasingDepartmentManager.Password = HashPassword(PurchasingDepartmentManager?.Password);

                _context.Add(PurchasingDepartmentManager);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Users");

            }
            return RedirectToAction("Index", "Users");
        }

        // GET: PurchasingDepartmentManagers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PurchasingDepartmentManager == null)
            {
                return NotFound();
            }

            var PurchasingDepartmentManager = await _context.PurchasingDepartmentManager.FindAsync(id);
            if (PurchasingDepartmentManager == null)
            {
                return NotFound();
            }
            return View(PurchasingDepartmentManager);
        }

        // POST: PurchasingDepartmentManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] PurchasingDepartmentManager PurchasingDepartmentManager)
        {
            if (id != PurchasingDepartmentManager.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == PurchasingDepartmentManager.Email && u.UserID != id);
                if (existingUser != null)
                {
                    // If user with the same email exists (and has a different ID), inform the user with a message
                    ModelState.AddModelError("Email", "Another user with this email already exists.");
                    return View(PurchasingDepartmentManager);
                }



                try
                {
                    _context.Update(PurchasingDepartmentManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchasingDepartmentManagerExists(PurchasingDepartmentManager.UserID))
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
            return View(PurchasingDepartmentManager);
        }

        // GET: PurchasingDepartmentManagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PurchasingDepartmentManager == null)
            {
                return NotFound();
            }

            var PurchasingDepartmentManager = await _context.PurchasingDepartmentManager
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (PurchasingDepartmentManager == null)
            {
                return NotFound();
            }

            return View(PurchasingDepartmentManager);
        }

        // POST: PurchasingDepartmentManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PurchasingDepartmentManager == null)
            {
                return Problem("Entity set 'GAPContext.PurchasingDepartmentManager'  is null.");
            }
            var PurchasingDepartmentManager = await _context.PurchasingDepartmentManager.FindAsync(id);
            if (PurchasingDepartmentManager != null)
            {
                _context.PurchasingDepartmentManager.Remove(PurchasingDepartmentManager);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchasingDepartmentManagerExists(int id)
        {
          return (_context.PurchasingDepartmentManager?.Any(e => e.UserID == id)).GetValueOrDefault();
        }




        private string HashPassword(string password)
        {
            // use a library like BCrypt or Argon2 to hash the password
            // here's an example using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


    }
}
