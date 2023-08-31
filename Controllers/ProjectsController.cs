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
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly GAPContext _context;

        public ProjectsController(GAPContext context)
        {
            _context = context;
        }





        // GET: Projects1
        [HttpGet("/Projects")]
        [SwaggerOperation(Summary = "Get projects", Description = "Retrieve a list of projects.")]
        [SwaggerResponse(200, "List of projects retrieved successfully.")]
        public async Task<IActionResult> Index(int? page, string SearchString)
        {


            IQueryable<Project> PurchaseQuoteiq = from o in _context.Project.Include(p=>p.Products) select o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                PurchaseQuoteiq = _context.Project.Where(o => o.Name.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await PurchaseQuoteiq.ToPagedListAsync(pageNumber, pageSize));


        }
        
        
        
        
        
        
        // GET: Projects1/Details/5
        [HttpGet("/Projects/Details/{id}")]
        [SwaggerOperation(Summary = "Get details of a project", Description = "Retrieve the details of a project.")]
        [SwaggerResponse(200, "Project details retrieved successfully.")]
        [SwaggerResponse(404, "Project not found.")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Project == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ProjectID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }





        // GET: Projects1/Create
        [HttpGet("/Projects/Create")]
        [SwaggerOperation(Summary = "Show project creation form", Description = "Display the project creation form.")]
        [SwaggerResponse(200, "Project creation form displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }





        // POST: Projects1/Create
        [HttpPost("/Projects/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new project", Description = "Create a new project.")]
        [SwaggerResponse(200, "Project created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<IActionResult> Create([Bind("ProjectID,StartDate,EndDate,Name,Description,Budget")] Project project)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var projectm= _context.ProjectManager.Include(p=>p.Projects).Where(p=>p.UserID == userId).FirstOrDefault();
                projectm.Projects.Add(project);
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }






        // GET: Projects1/Edit/5
        [HttpGet("/Projects/Edit/{id}")]
        [SwaggerOperation(Summary = "Show project edit form", Description = "Display the project edit form.")]
        [SwaggerResponse(200, "Project edit form displayed successfully.")]
        [SwaggerResponse(404, "Project not found.")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Project == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }







        // POST: Projects1/Edit/5
        [HttpPost("/Projects/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit a project", Description = "Edit an existing project.")]
        [SwaggerResponse(200, "Project edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Project not found.")]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectID,StartDate,EndDate,Name,Description,Budget")] Project project)
        {
            if (id != project.ProjectID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectID))
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
            return View(project);
        }






        // GET: Projects1/Delete/5
        [HttpGet("/Projects/Delete/{id}")]
        [SwaggerOperation(Summary = "Show project delete confirmation", Description = "Display the project delete confirmation.")]
        [SwaggerResponse(200, "Project delete confirmation displayed successfully.")]
        [SwaggerResponse(404, "Project not found.")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Project == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ProjectID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }








        // POST: Projects1/Delete/5
        [HttpPost("/Projects/DeleteConfirmed/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete a project", Description = "Delete an existing project.")]
        [SwaggerResponse(200, "Project deleted successfully.")]
        [SwaggerResponse(404, "Project not found.")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Project == null)
            {
                return Problem("Entity set 'GAPContext.Project'  is null.");
            }
            var project = await _context.Project.FindAsync(id);
            if (project != null)
            {
                _context.Project.Remove(project);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }







        /*---------------------------------------------------------------*/






        // Helper: no route
        private bool ProjectExists(int id)
        {
          return (_context.Project?.Any(e => e.ProjectID == id)).GetValueOrDefault();
        }
    }
}
