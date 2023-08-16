using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using System.Security.Claims;

namespace GAP.Controllers
{
    public class ReclamationsController : Controller
    {
        private readonly GAPContext _context;

        public ReclamationsController(GAPContext context)
        {
            _context = context;
        }

        // GET: Reclamations
        public async Task<IActionResult> Indexusers()
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var reclamations = _context.Reclamation.Where(r=>r.UserID==userId).ToList();
            var reclamationsWithRepliesAndUsers = new List<dynamic>();

            foreach (var reclamation in reclamations)
            {
                var user = _context.User.FirstOrDefault(u => u.UserID == reclamation.UserID);
                var reclamationReply = _context.ReclamationReply.Where(rr => rr.ReclamationID == reclamation.ReclamationID).ToList();

                var reclamationData = new
                {
                    Reclamation = reclamation,
                    User = user,
                    ReclamationReply = reclamationReply
                };

                reclamationsWithRepliesAndUsers.Add(reclamationData);
            }

            ViewBag.ReclamationsWithRepliesAndUsers = reclamationsWithRepliesAndUsers;

            return View();

        }



        // GET: Reclamations/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReclamationTitle,Description,BugPicture")] Reclamation reclamation, IFormFile bugPicture)
        {
            if (ModelState.IsValid)
            {
                if (bugPicture != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await bugPicture.CopyToAsync(memoryStream);
                        reclamation.BugPicture = memoryStream.ToArray();
                    }
                }
                else
                {
                    // Load the default replacement picture
                    string defaultImagePath = @"C:\Users\yahya\Desktop\GAP\Code Source\GAP\wwwroot\images\bug.png";
                    reclamation.BugPicture = System.IO.File.ReadAllBytes(defaultImagePath);
                }

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                reclamation.UserID = userId;
                reclamation.CreationDate = DateTime.Now.Date;

                _context.Add(reclamation);
                await _context.SaveChangesAsync(); // Save the new Reclamation to get its ID

                var reclamationsHistory = new ReclamationsHistory();
                reclamationsHistory.ReclamationsID = reclamation.ReclamationID;

                _context.Add(reclamationsHistory);
                await _context.SaveChangesAsync(); // Save the new ReclamationsHistory

                return RedirectToAction("Indexusers", "Reclamations");
            }

            return RedirectToAction("Indexusers", "Reclamations");
        }





        // POST: ReclamationReplies/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReclamation(int ReclamationID)
        {
            try
            {
                if (_context.ReclamationReply == null)
                {
                    return Problem("Entity set 'GAPContext.ReclamationReply' is null.");
                }

                var reclamation = await _context.Reclamation.FindAsync(ReclamationID);

                if (reclamation != null)
                {
                    // Get all the reclamation replies associated with this reclamation
                    var reclamationReplies = _context.ReclamationReply
                        .Where(reply => reply.ReclamationID == reclamation.ReclamationID);

                    // Remove all the associated reclamation replies
                    _context.ReclamationReply.RemoveRange(reclamationReplies);

                    // Remove the reclamation itself
                    _context.Reclamation.Remove(reclamation);

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Indexusers", "Reclamations");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the deletion process
                return RedirectToAction("Indexusers", "Reclamations");
            }
        }


        private bool ReclamationExists(int id)
        {
          return (_context.Reclamation?.Any(e => e.ReclamationID == id)).GetValueOrDefault();
        }
    }
}
