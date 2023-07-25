using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using X.PagedList;

namespace GAP.Controllers
{
    [Authorize]
    public class DemandeAchatsController : Controller
    {
        private readonly GAPContext _context;

        public DemandeAchatsController(GAPContext context)
        {
            _context = context;
        }

        // GET: DemandeAchats
        [Authorize(Roles = "RespServiceAchat")]

        public async Task<IActionResult> Index(int? page ,string SearchString)
        {
           

            IQueryable<DemandeAchat> DemandeAchatiq = from s in _context.DemandeAchat
                                      select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                DemandeAchatiq = _context.DemandeAchat.Where(d => d.Description.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await DemandeAchatiq.ToPagedListAsync(pageNumber, pageSize));


        }
        // GET: DemandeAchats for fournisseur, this method will be allowed for the fournisseur role
        [Authorize(Roles = "Fournisseur")]
        public async Task<IActionResult> IndexFour(int? page, string SearchString)
        {


            IQueryable<DemandeAchat> DemandeAchatiq = from s in _context.DemandeAchat
                                                      select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                DemandeAchatiq = _context.DemandeAchat.Where(d => d.Description.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(await DemandeAchatiq.ToPagedListAsync(pageNumber, pageSize));
        }


        // GET: DemandeAchats/Details/5
        [Authorize(Roles = "RespServiceAchat")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DemandeAchat == null)
            {
                return NotFound();
            }

            var demandeAchat = await _context.DemandeAchat
                .FirstOrDefaultAsync(m => m.DemandeAchatID == id);
            if (demandeAchat == null)
            {
                return NotFound();
            }

            return View(demandeAchat);
        }

        // GET: DemandeAchats/Create
        [Authorize(Roles = "RespServiceAchat")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: DemandeAchats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "RespServiceAchat")]

        public async Task<IActionResult> Create([Bind("DemandeAchatID,Description,Budget")] DemandeAchat demandeAchat)
        {
            if (ModelState.IsValid)
            {
                demandeAchat.CreationDate = DateTime.Now;
                _context.Add(demandeAchat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(demandeAchat);
        }

        // GET: DemandeAchats/Edit/5
        [Authorize(Roles = "RespServiceAchat")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DemandeAchat == null)
            {
                return NotFound();
            }

            var demandeAchat = await _context.DemandeAchat.FindAsync(id);
            if (demandeAchat == null)
            {
                return NotFound();
            }
            return View(demandeAchat);
        }

        // POST: DemandeAchats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "RespServiceAchat")]

        public async Task<IActionResult> Edit(int id, [Bind("DemandeAchatID,CreationDate,Description,Budget")] DemandeAchat demandeAchat)
        {
            if (id != demandeAchat.DemandeAchatID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(demandeAchat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DemandeAchatExists(demandeAchat.DemandeAchatID))
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
            return View(demandeAchat);
        }

        // GET: DemandeAchats/Delete/5
        [Authorize(Roles = "RespServiceAchat")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DemandeAchat == null)
            {
                return NotFound();
            }

            var demandeAchat = await _context.DemandeAchat
                .FirstOrDefaultAsync(m => m.DemandeAchatID == id);
            if (demandeAchat == null)
            {
                return NotFound();
            }

            return View(demandeAchat);
        }

        // POST: DemandeAchats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "RespServiceAchat")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DemandeAchat == null)
            {
                return Problem("Entity set 'GAPContext.DemandeAchat'  is null.");
            }
            var demandeAchat = await _context.DemandeAchat.FindAsync(id);
            if (demandeAchat != null)
            {
                _context.DemandeAchat.Remove(demandeAchat);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DemandeAchatExists(int id)
        {
          return (_context.DemandeAchat?.Any(e => e.DemandeAchatID == id)).GetValueOrDefault();
        }
    }
}
