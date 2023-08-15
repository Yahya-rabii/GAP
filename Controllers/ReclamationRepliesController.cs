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

namespace GAP.Controllers
{

    [Authorize]
    [Authorize(Roles = "Admin")]
    public class ReclamationRepliesController : Controller
    {
        private readonly GAPContext _context;

        public ReclamationRepliesController(GAPContext context)
        {
            _context = context;
        }

        // GET: ReclamationReplies
        public async Task<IActionResult> Indexadmin()
        {
            var reclamations = _context.Reclamation.ToList();
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


  

        // GET: ReclamationReplies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReclamationReplies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReclamationReplyID,ReclamationID,Answer")] ReclamationReply reclamationReply)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reclamationReply);
                await _context.SaveChangesAsync();
                return RedirectToAction("Indexadmin", "ReclamationReplies");

            }
            return RedirectToAction("Indexadmin", "ReclamationReplies");
        }

      


        // POST: ReclamationReplies/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReply(int ReclamationReplyID)
        {
            if (_context.ReclamationReply == null)
            {
                return Problem("Entity set 'GAPContext.ReclamationReply'  is null.");
            }
            var reclamationReply = await _context.ReclamationReply.FindAsync(ReclamationReplyID);
            if (reclamationReply != null)
            {
                _context.ReclamationReply.Remove(reclamationReply);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Indexadmin", "ReclamationReplies");

        }

        // POST: ReclamationReplies/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReclamationAD(int ReclamationID)
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

                return RedirectToAction("Indexadmin", "ReclamationReplies");

            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the deletion process
                return RedirectToAction("Indexadmin", "ReclamationReplies");

            }
        }


        private bool ReclamationReplyExists(int id)
        {
          return (_context.ReclamationReply?.Any(e => e.ReclamationReplyID == id)).GetValueOrDefault();
        }
    }
}
