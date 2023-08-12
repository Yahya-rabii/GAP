using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;

namespace GAP.Controllers
{
    public class ProjectManagersController : Controller
    {
        private readonly GAPContext _context;

        public ProjectManagersController(GAPContext context)
        {
            _context = context;
        }

        // GET: ProjectManagers
        public async Task<IActionResult> Index()
        {
              return _context.ProjectManager != null ? 
                          View(await _context.ProjectManager.ToListAsync()) :
                          Problem("Entity set 'GAPContext.ProjectManager'  is null.");
        }

        // GET: ProjectManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProjectManager == null)
            {
                return NotFound();
            }

            var projectManager = await _context.ProjectManager
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (projectManager == null)
            {
                return NotFound();
            }

            return View(projectManager);
        }

        // GET: ProjectManagers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProjectManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] ProjectManager ProjectManager)
        {
            if (ModelState.IsValid)
            {

                // Check if the user with the provided email already exists in the database
                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == ProjectManager.Email);
                if (existingUser != null)
                {
                    // If user with the same email exists, inform the user with a message
                    ModelState.AddModelError("Email", "User with this email already exists.");
                    return View(ProjectManager);
                }
                ProjectManager.Password = HashPassword(ProjectManager?.Password);

                _context.Add(ProjectManager);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Users");

            }
            return RedirectToAction("Index", "Users");
        }

        // POST: ProjectManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectManagerID,ProjectId,UserID,Email,Password,FirstName,LastName,IsAdmin,Role,ProfilePicture,ProfilePictureFileName,HasCustomProfilePicture")] ProjectManager projectManager)
        {
            if (id != projectManager.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectManagerExists(projectManager.UserID))
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
            return View(projectManager);
        }

        // GET: ProjectManagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProjectManager == null)
            {
                return NotFound();
            }

            var projectManager = await _context.ProjectManager
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (projectManager == null)
            {
                return NotFound();
            }

            return View(projectManager);
        }

        // POST: ProjectManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProjectManager == null)
            {
                return Problem("Entity set 'GAPContext.ProjectManager'  is null.");
            }
            var projectManager = await _context.ProjectManager.FindAsync(id);
            if (projectManager != null)
            {
                _context.ProjectManager.Remove(projectManager);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectManagerExists(int id)
        {
          return (_context.ProjectManager?.Any(e => e.UserID == id)).GetValueOrDefault();
        }



        private string HashPassword(string password)
        {
            // use a library like BCrypt or Argon2 to hash the password
            // here's an example using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

    }
}
