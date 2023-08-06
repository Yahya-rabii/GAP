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
    public class QualityTestingDepartmentManagersController : Controller
    {
        private readonly GAPContext _context;

        public QualityTestingDepartmentManagersController(GAPContext context)
        {
            _context = context;
        }

        // GET: QualityTestingDepartmentManagers
        public async Task<IActionResult> Index(string SearchString, int? page)
        {
            IQueryable<QualityTestingDepartmentManager> iseriq = from s in _context.QualityTestingDepartmentManager
                                                    select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                iseriq = _context.QualityTestingDepartmentManager.Where(s => s.Email.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: QualityTestingDepartmentManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.QualityTestingDepartmentManager == null)
            {
                return NotFound();
            }

            var QualityTestingDepartmentManager = await _context.QualityTestingDepartmentManager
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (QualityTestingDepartmentManager == null)
            {
                return NotFound();
            }

            return View(QualityTestingDepartmentManager);
        }

        // GET: QualityTestingDepartmentManagers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QualityTestingDepartmentManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] QualityTestingDepartmentManager QualityTestingDepartmentManager)
        {
            if (ModelState.IsValid)
            {

                // Check if the user with the provided email already exists in the database
                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == QualityTestingDepartmentManager.Email);
                if (existingUser != null)
                {
                    // If user with the same email exists, inform the user with a message
                    ModelState.AddModelError("Email", "User with this email already exists.");
                    return View(QualityTestingDepartmentManager);
                }

                QualityTestingDepartmentManager.Password = HashPassword(QualityTestingDepartmentManager?.Password);

                _context.Add(QualityTestingDepartmentManager);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Users");
            }
            return RedirectToAction("Index", "Users");
        }

        // GET: QualityTestingDepartmentManagers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.QualityTestingDepartmentManager == null)
            {
                return NotFound();
            }

            var QualityTestingDepartmentManager = await _context.QualityTestingDepartmentManager.FindAsync(id);
            if (QualityTestingDepartmentManager == null)
            {
                return NotFound();
            }
            return View(QualityTestingDepartmentManager);
        }

        // POST: QualityTestingDepartmentManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] QualityTestingDepartmentManager QualityTestingDepartmentManager)
    {
        if (id != QualityTestingDepartmentManager.UserID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Check if the user with the provided email already exists in the database
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == QualityTestingDepartmentManager.Email && u.UserID != id);
            if (existingUser != null)
            {
                // If user with the same email exists (and has a different ID), inform the user with a message
                ModelState.AddModelError("Email", "Another user with this email already exists.");
                return View(QualityTestingDepartmentManager);
            }

            try
            {
                _context.Update(QualityTestingDepartmentManager);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QualityTestingDepartmentManagerExists(QualityTestingDepartmentManager.UserID))
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
        return View(QualityTestingDepartmentManager);
    }


    // GET: QualityTestingDepartmentManagers/Delete/5
    public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.QualityTestingDepartmentManager == null)
            {
                return NotFound();
            }

            var QualityTestingDepartmentManager = await _context.QualityTestingDepartmentManager
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (QualityTestingDepartmentManager == null)
            {
                return NotFound();
            }

            return View(QualityTestingDepartmentManager);
        }

        // POST: QualityTestingDepartmentManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.QualityTestingDepartmentManager == null)
            {
                return Problem("Entity set 'GAPContext.QualityTestingDepartmentManager'  is null.");
            }
            var QualityTestingDepartmentManager = await _context.QualityTestingDepartmentManager.FindAsync(id);
            if (QualityTestingDepartmentManager != null)
            {
                _context.QualityTestingDepartmentManager.Remove(QualityTestingDepartmentManager);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QualityTestingDepartmentManagerExists(int id)
        {
          return (_context.QualityTestingDepartmentManager?.Any(e => e.UserID == id)).GetValueOrDefault();
        }




        private string HashPassword(string password)
        {
            // use a library like BCrypt or Argon2 to hash the password
            // here's an example using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
