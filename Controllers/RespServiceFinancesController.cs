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
    public class RespServiceFinancesController : Controller
    {
        private readonly GAPContext _context;

        public RespServiceFinancesController(GAPContext context)
        {
            _context = context;
        }

        // GET: RespServiceFinances
        public async Task<IActionResult> Index()
        {
              return _context.RespServiceFinance != null ? 
                          View(await _context.RespServiceFinance.ToListAsync()) :
                          Problem("Entity set 'GAPContext.RespServiceFinance'  is null.");
        }

        // GET: RespServiceFinances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RespServiceFinance == null)
            {
                return NotFound();
            }

            var respServiceFinance = await _context.RespServiceFinance
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (respServiceFinance == null)
            {
                return NotFound();
            }

            return View(respServiceFinance);
        }

        // GET: RespServiceFinances/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RespServiceFinances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] RespServiceFinance respServiceFinance)
        {
            if (ModelState.IsValid)
            {
                // Check if the user with the provided email already exists in the database
                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == respServiceFinance.Email);
                if (existingUser != null)
                {
                    // If user with the same email exists, inform the user with a message
                    ModelState.AddModelError("Email", "User with this email already exists.");
                    return View(respServiceFinance);
                }

                respServiceFinance.Password = HashPassword(respServiceFinance?.Password);

                _context.Add(respServiceFinance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(respServiceFinance);
        }

        // GET: RespServiceFinances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RespServiceFinance == null)
            {
                return NotFound();
            }

            var respServiceFinance = await _context.RespServiceFinance.FindAsync(id);
            if (respServiceFinance == null)
            {
                return NotFound();
            }
            return View(respServiceFinance);
        }

        // POST: RespServiceFinances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] RespServiceFinance respServiceFinance)
        {
            if (id != respServiceFinance.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == respServiceFinance.Email && u.UserID != id);
                if (existingUser != null)
                {
                    // If user with the same email exists (and has a different ID), inform the user with a message
                    ModelState.AddModelError("Email", "Another user with this email already exists.");
                    return View(respServiceFinance);
                }

                try
                {
                    _context.Update(respServiceFinance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RespServiceFinanceExists(respServiceFinance.UserID))
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
            return View(respServiceFinance);
        }

        // GET: RespServiceFinances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RespServiceFinance == null)
            {
                return NotFound();
            }

            var respServiceFinance = await _context.RespServiceFinance
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (respServiceFinance == null)
            {
                return NotFound();
            }

            return View(respServiceFinance);
        }

        // POST: RespServiceFinances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RespServiceFinance == null)
            {
                return Problem("Entity set 'GAPContext.RespServiceFinance'  is null.");
            }
            var respServiceFinance = await _context.RespServiceFinance.FindAsync(id);
            if (respServiceFinance != null)
            {
                _context.RespServiceFinance.Remove(respServiceFinance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RespServiceFinanceExists(int id)
        {
          return (_context.RespServiceFinance?.Any(e => e.UserID == id)).GetValueOrDefault();
        }




        private string HashPassword(string password)
        {
            // use a library like BCrypt or Argon2 to hash the password
            // here's an example using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }










        public IActionResult Home()
        {
            // Implementation for creating an Offre
            return View();
        }

















    }
}
