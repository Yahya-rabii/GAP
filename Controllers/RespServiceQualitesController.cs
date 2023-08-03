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
    public class RespServiceQualitesController : Controller
    {
        private readonly GAPContext _context;

        public RespServiceQualitesController(GAPContext context)
        {
            _context = context;
        }

        // GET: RespServiceQualites
        public async Task<IActionResult> Index(string SearchString, int? page)
        {
            IQueryable<RespServiceQualite> iseriq = from s in _context.RespServiceQualite
                                                    select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                iseriq = _context.RespServiceQualite.Where(s => s.Email.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: RespServiceQualites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RespServiceQualite == null)
            {
                return NotFound();
            }

            var respServiceQualite = await _context.RespServiceQualite
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (respServiceQualite == null)
            {
                return NotFound();
            }

            return View(respServiceQualite);
        }

        // GET: RespServiceQualites/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RespServiceQualites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] RespServiceQualite respServiceQualite)
        {
            if (ModelState.IsValid)
            {

                // Check if the user with the provided email already exists in the database
                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == respServiceQualite.Email);
                if (existingUser != null)
                {
                    // If user with the same email exists, inform the user with a message
                    ModelState.AddModelError("Email", "User with this email already exists.");
                    return View(respServiceQualite);
                }

                respServiceQualite.Password = HashPassword(respServiceQualite?.Password);

                _context.Add(respServiceQualite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(respServiceQualite);
        }

        // GET: RespServiceQualites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RespServiceQualite == null)
            {
                return NotFound();
            }

            var respServiceQualite = await _context.RespServiceQualite.FindAsync(id);
            if (respServiceQualite == null)
            {
                return NotFound();
            }
            return View(respServiceQualite);
        }

        // POST: RespServiceQualites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] RespServiceQualite respServiceQualite)
    {
        if (id != respServiceQualite.UserID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Check if the user with the provided email already exists in the database
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == respServiceQualite.Email && u.UserID != id);
            if (existingUser != null)
            {
                // If user with the same email exists (and has a different ID), inform the user with a message
                ModelState.AddModelError("Email", "Another user with this email already exists.");
                return View(respServiceQualite);
            }

            try
            {
                _context.Update(respServiceQualite);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RespServiceQualiteExists(respServiceQualite.UserID))
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
        return View(respServiceQualite);
    }


    // GET: RespServiceQualites/Delete/5
    public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RespServiceQualite == null)
            {
                return NotFound();
            }

            var respServiceQualite = await _context.RespServiceQualite
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (respServiceQualite == null)
            {
                return NotFound();
            }

            return View(respServiceQualite);
        }

        // POST: RespServiceQualites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RespServiceQualite == null)
            {
                return Problem("Entity set 'GAPContext.RespServiceQualite'  is null.");
            }
            var respServiceQualite = await _context.RespServiceQualite.FindAsync(id);
            if (respServiceQualite != null)
            {
                _context.RespServiceQualite.Remove(respServiceQualite);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RespServiceQualiteExists(int id)
        {
          return (_context.RespServiceQualite?.Any(e => e.UserID == id)).GetValueOrDefault();
        }




        private string HashPassword(string password)
        {
            // use a library like BCrypt or Argon2 to hash the password
            // here's an example using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
