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
using Humanizer;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GAP.Controllers
{
    public class ReceptServiceAchatsController : Controller
    {
        private readonly GAPContext _context;

        public ReceptServiceAchatsController(GAPContext context)
        {
            _context = context;
        }

        // GET: ReceptServiceAchats
        public async Task<IActionResult> Index()
        {
              return _context.ReceptServiceAchat != null ? 
                          View(await _context.ReceptServiceAchat.ToListAsync()) :
                          Problem("Entity set 'GAPContext.ReceptServiceAchat'  is null.");
        }

        // GET: ReceptServiceAchats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReceptServiceAchat == null)
            {
                return NotFound();
            }

            var receptServiceAchat = await _context.ReceptServiceAchat
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (receptServiceAchat == null)
            {
                return NotFound();
            }

            return View(receptServiceAchat);
        }

        // GET: ReceptServiceAchats/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] ReceptServiceAchat receptServiceAchat)
        {
            if (ModelState.IsValid)
            {
                // Check if the user with the provided email already exists in the database
                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == receptServiceAchat.Email);
                if (existingUser != null)
                {
                    // If user with the same email exists, inform the user with a message
                    ModelState.AddModelError("Email", "User with this email already exists.");
                    return View(receptServiceAchat);
                }

                // If the user does not exist, proceed with adding them to the database
                receptServiceAchat.Password = HashPassword(receptServiceAchat?.Password);
                _context.Add(receptServiceAchat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(receptServiceAchat);
        }



        // GET: ReceptServiceAchats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReceptServiceAchat == null)
            {
                return NotFound();
            }

            var receptServiceAchat = await _context.ReceptServiceAchat.FindAsync(id);
            if (receptServiceAchat == null)
            {
                return NotFound();
            }
            return View(receptServiceAchat);
        }

        // POST: ReceptServiceAchats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] ReceptServiceAchat receptServiceAchat)
        {
            if (id != receptServiceAchat.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == receptServiceAchat.Email && u.UserID != id);
                if (existingUser != null)
                {
                    // If user with the same email exists (and has a different ID), inform the user with a message
                    ModelState.AddModelError("Email", "Another user with this email already exists.");
                    return View(receptServiceAchat);
                }



                try
                {
                    _context.Update(receptServiceAchat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceptServiceAchatExists(receptServiceAchat.UserID))
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
            return View(receptServiceAchat);
        }

        // GET: ReceptServiceAchats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReceptServiceAchat == null)
            {
                return NotFound();
            }

            var receptServiceAchat = await _context.ReceptServiceAchat
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (receptServiceAchat == null)
            {
                return NotFound();
            }

            return View(receptServiceAchat);
        }

        // POST: ReceptServiceAchats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReceptServiceAchat == null)
            {
                return Problem("Entity set 'GAPContext.ReceptServiceAchat'  is null.");
            }
            var receptServiceAchat = await _context.ReceptServiceAchat.FindAsync(id);
            if (receptServiceAchat != null)
            {
                _context.ReceptServiceAchat.Remove(receptServiceAchat);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceptServiceAchatExists(int id)
        {
          return (_context.ReceptServiceAchat?.Any(e => e.UserID == id)).GetValueOrDefault();
        }



        private string HashPassword(string password)
        {
            // use a library like BCrypt or Argon2 to hash the password
            // here's an example using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


       




    }
}
