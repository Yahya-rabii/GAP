﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using GAP.Helper;
using Microsoft.AspNetCore.Authorization;
using System.Data;
namespace GAP.Controllers

{
    [Authorize]
    [Authorize(Roles = "Admin")]
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
                .FirstOrDefaultAsync(m => m.ReceptServiceAchatID == id);
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

        // POST: ReceptServiceAchats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReceptServiceAchatID,Email,Password,FirstName,LastName")] ReceptServiceAchat receptServiceAchat)
        {

            

            if (ModelState.IsValid)
            {
                // test if the user receptServiceAchat exist already if so add it to the table if not raise an exeption or show an error to the user

                // Check if the user already exists by email
                bool userExists = await _context.HistoryU.AnyAsync(r => r.Email == receptServiceAchat.Email);
                if (userExists)
                {
                    ModelState.AddModelError("Email", "receptServiceAchat with this email already exists.");
                    return View(receptServiceAchat);
                }
               
                

                ReceptServiceAchat receptServiceAchat1 = new(

                receptServiceAchat.ReceptServiceAchatID,
                receptServiceAchat.Email,
                receptServiceAchat.Password,
                receptServiceAchat.FirstName,
                receptServiceAchat.LastName

                );

                HistoryU historyU = new(
                    receptServiceAchat.ReceptServiceAchatID,
                    receptServiceAchat.Email,
                    "receptServiceAchat"

                    );

                _context.HistoryU.Add(historyU);
                _context.Add(receptServiceAchat1);
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
        public async Task<IActionResult> Edit(int id, [Bind("ReceptServiceAchatID,Email,Password,FirstName,LastName")] ReceptServiceAchat receptServiceAchat)
        {
            if (id != receptServiceAchat.ReceptServiceAchatID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receptServiceAchat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceptServiceAchatExists(receptServiceAchat.ReceptServiceAchatID))
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
                .FirstOrDefaultAsync(m => m.ReceptServiceAchatID == id);
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
          return (_context.ReceptServiceAchat?.Any(e => e.ReceptServiceAchatID == id)).GetValueOrDefault();
        }
    }
}
