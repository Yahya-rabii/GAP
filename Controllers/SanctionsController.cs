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
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Controllers
{
    public class SanctionsController : Controller
    {
        private readonly GAPContext _context;

        public SanctionsController(GAPContext context)
        {
            _context = context;
        }



        // GET: Sanctions
        [HttpGet("/Sanctions")]
        [SwaggerOperation(Summary = "Get a list of sanctions", Description = "Retrieve a list of sanctions.")]
        [SwaggerResponse(200, "List of sanctions retrieved successfully.")]
        public async Task<IActionResult> Index(string SearchString, int? page)
        {
            IQueryable<Sanction> iseriq = from s in _context.Sanction
                                      select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                iseriq = _context.Sanction.Where(s => s.SupplierId == _context.Supplier.Where(ss => ss.Email.ToLower().Contains(SearchString.ToLower().Trim())).FirstOrDefault().UserID) ;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        }






        // GET: Sanctions/Details/5
        [HttpGet("/Sanctions/Details/{id}")]
        [SwaggerOperation(Summary = "Get sanction details", Description = "Retrieve details of a sanction.")]
        [SwaggerResponse(200, "Sanction details retrieved successfully.")]
        [SwaggerResponse(404, "Sanction not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sanction == null)
            {
                return NotFound();
            }

            var sanction = await _context.Sanction
                .FirstOrDefaultAsync(m => m.SanctionID == id);
            if (sanction == null)
            {
                return NotFound();
            }

            return View(sanction);
        }







        // GET: Sanctions/Create
        [HttpGet("/Sanctions/Create")]
        [SwaggerOperation(Summary = "Show sanction creation form", Description = "Display the sanction creation form.")]
        [SwaggerResponse(200, "Sanction creation form displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }







        // POST: Sanctions/Create
        [HttpPost("/Sanctions/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new sanction", Description = "Create a new sanction with the provided information.")]
        [SwaggerResponse(200, "Sanction created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Create([Bind("SanctionID,SanctionTitle,SanctionDescription,SupplierId")] Sanction sanction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sanction);
        }






        // GET: Sanctions/Edit/5
        [HttpGet("/Sanctions/Edit/{id}")]
        [SwaggerOperation(Summary = "Show sanction editing form", Description = "Display the form for editing a sanction's information.")]
        [SwaggerResponse(200, "Sanction editing form displayed successfully.")]
        [SwaggerResponse(404, "Sanction not found.")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sanction == null)
            {
                return NotFound();
            }

            var sanction = await _context.Sanction.FindAsync(id);
            if (sanction == null)
            {
                return NotFound();
            }
            return View(sanction);
        }





        // POST: Sanctions/Edit/5
        [HttpPost("/Sanctions/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit sanction information", Description = "Edit the information of a sanction.")]
        [SwaggerResponse(200, "Sanction information edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Sanction not found.")]
        public async Task<IActionResult> Edit(int id, [Bind("SanctionID,SanctionTitle,SanctionDescription,SupplierId")] Sanction sanction)
        {
            if (id != sanction.SanctionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanctionExists(sanction.SanctionID))
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
            return View(sanction);
        }





        // GET: Sanctions/Delete/5
        [HttpGet("/Sanctions/Delete/{id}")]
        [SwaggerOperation(Summary = "Show sanction deleting form", Description = "Display the form for deleting a sanction's information.")]
        [SwaggerResponse(200, "Sanction deleting form displayed successfully.")]
        [SwaggerResponse(404, "Sanction not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sanction == null)
            {
                return NotFound();
            }

            var sanction = await _context.Sanction
                .FirstOrDefaultAsync(m => m.SanctionID == id);
            if (sanction == null)
            {
                return NotFound();
            }

            return View(sanction);
        }






        // POST: Sanctions/Delete/5
        [HttpPost("/Sanctions/Delete/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete sanction", Description = "Delete a sanction.")]
        [SwaggerResponse(200, "Sanction deleted successfully.")]
        [SwaggerResponse(404, "Sanction not found.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sanction == null)
            {
                return Problem("Entity set 'GAPContext.Sanction'  is null.");
            }
            var sanction = await _context.Sanction.FindAsync(id);
            if (sanction != null)
            {
                _context.Sanction.Remove(sanction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        /*---------------------------------------------------------------*/




        // Helper: no route
        private bool SanctionExists(int id)
        {
          return (_context.Sanction?.Any(e => e.SanctionID == id)).GetValueOrDefault();
        }
    }
}
