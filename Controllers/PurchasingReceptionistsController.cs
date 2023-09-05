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
using X.PagedList;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GAP.Controllers
{
    public class PurchasingReceptionistsController : Controller
    {
        private readonly GAPContext _context;

        public PurchasingReceptionistsController(GAPContext context)
        {
            _context = context;
        }

        // GET: PurchasingReceptionists
        public async Task<IActionResult> Index(string SearchString, int? page)
        {
            IQueryable<PurchasingReceptionist> iseriq = from s in _context.PurchasingReceptionist
                                                    select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                iseriq = _context.PurchasingReceptionist.Where(s => s.Email.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));
        }


        ////////////////////////////////////////////////////////////////////////////////////

        public async Task<IActionResult> IndexPurchaseQuote(int? page, string SearchString)
        {
            // Query to retrieve PurchaseQuotes that are not added to the reception report
            IQueryable<PurchaseQuote> PurchaseQuoteiq = _context.PurchaseQuote.Include(o => o.Supplier).Include(o => o.Products)
                                                        .Where(pq => !_context.ReceptionReport.Any(rr => rr.PurchaseQuoteId == pq.PurchaseQuoteID));

            if (!string.IsNullOrEmpty(SearchString))
            {
                PurchaseQuoteiq = PurchaseQuoteiq.Where(o => o.Supplier.Email.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);

            // Load RespServiceFinnance and RespServiceQalite users and store them in ViewBag
            ViewBag.RespServiceFinnanceUsers = await _context.FinanceDepartmentManager.ToListAsync();
            ViewBag.RespServiceQaliteUsers = await _context.QualityTestingDepartmentManager.ToListAsync();

            // Materialize the factiq query into a list to avoid the DataReader conflict
            List<BillPurchase> BillsList = await _context.BillPurchase.ToListAsync();
            List<Sanction> sanctions = await _context.Sanction.ToListAsync();

            // Iterate through the PurchaseQuote items and check if a corresponding Bill exists
            foreach (var PurchaseQuoteItem in PurchaseQuoteiq)
            {
                // Check if a Bill exists for the current PurchaseQuote item and if the ReceptionDate is less than the current date
                if (!BillsList.Any(f => f.PurchaseQuoteID == PurchaseQuoteItem.PurchaseQuoteID) && PurchaseQuoteItem.ReceptionDate.Date < DateTime.Now.Date && !sanctions.Any(f => f.PurchaseQuoteID == PurchaseQuoteItem.PurchaseQuoteID))
                {
                    // A Bill does not exist, and ReceptionDate is less than the current date, so create a new Sanction
                    Sanction s = new Sanction();

                    s.SanctionTitle = "Delay Report";
                    s.SanctionDescription = "L'arrivage de Product est en retard";
                    s.SupplierId = PurchaseQuoteItem.SupplierID;
                    s.PurchaseQuoteID = PurchaseQuoteItem.PurchaseQuoteID;
                    _context.Sanction.Add(s);
                }
            }

            ViewBag.Bills = BillsList;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return View(await PurchaseQuoteiq.ToPagedListAsync(pageNumber, pageSize));
        }




                public async Task<IActionResult> IndexServiceQuote(int? page, string SearchString)
        {
            // Query to retrieve PurchaseQuotes that are not added to the reception report
            IQueryable<ServiceQuote> PurchaseQuoteiq = _context.ServiceQuote.Include(o => o.Supplier)
                                                        .Where(pq => !_context.ReceptionReport.Any(rr => rr.ServiceQuoteId == pq.ServiceQuoteID));

            if (!string.IsNullOrEmpty(SearchString))
            {
                PurchaseQuoteiq = PurchaseQuoteiq.Where(o => o.Supplier.Email.ToLower().Contains(SearchString.ToLower().Trim()));
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);

            // Load RespServiceFinnance and RespServiceQalite users and store them in ViewBag
            ViewBag.RespServiceFinnanceUsers = await _context.FinanceDepartmentManager.ToListAsync();
            ViewBag.RespServiceQaliteUsers = await _context.QualityTestingDepartmentManager.ToListAsync();

            // Materialize the factiq query into a list to avoid the DataReader conflict
            List<BillPurchase> BillsList = await _context.BillPurchase.ToListAsync();
            List<Sanction> sanctions = await _context.Sanction.ToListAsync();

            // Iterate through the PurchaseQuote items and check if a corresponding Bill exists
            foreach (var PurchaseQuoteItem in PurchaseQuoteiq)
            {
                // Check if a Bill exists for the current PurchaseQuote item and if the ReceptionDate is less than the current date
                if (!BillsList.Any(f => f.PurchaseQuoteID == PurchaseQuoteItem.ServiceQuoteID) && PurchaseQuoteItem.EndDate.Date < DateTime.Now.Date && !sanctions.Any(f => f.PurchaseQuoteID == PurchaseQuoteItem.ServiceQuoteID))
                {
                    // A Bill does not exist, and ReceptionDate is less than the current date, so create a new Sanction
                    Sanction s = new Sanction();

                    s.SanctionTitle = "Delay Report";
                    s.SanctionDescription = "L'arrivage de Product est en retard";
                    s.SupplierId = PurchaseQuoteItem.SupplierID;
                    s.PurchaseQuoteID = PurchaseQuoteItem.ServiceQuoteID;
                    _context.Sanction.Add(s);
                }
            }

            ViewBag.Bills = BillsList;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return View(await PurchaseQuoteiq.ToPagedListAsync(pageNumber, pageSize));
        }



        [HttpPost]
        public IActionResult CreateNotification(int PurchaseQuoteId, string respFinnanceEmail, string respQaliteEmail)
        {
            // Find the user IDs based on the selected emails
            var respFinnanceUser = _context.User.FirstOrDefault(u => u.Email == respFinnanceEmail);
            var respQaliteUser = _context.User.FirstOrDefault(u => u.Email == respQaliteEmail);

            if (respFinnanceUser != null && respQaliteUser != null)
            {
                // Create and save notification for RespServiceFinnance
                var notificationFinnance = new NotificationInfo
                {
                    PurchaseQuoteID = PurchaseQuoteId,
                    UserID = respFinnanceUser.UserID,
                    NotificationTitle = "Reclamation Notification Finnanciere"
                };
                _context.Notification.Add(notificationFinnance);

                // Create and save notification for RespServiceQalite
                var notificationQalite = new NotificationInfo
                {
                    PurchaseQuoteID = PurchaseQuoteId,
                    UserID = respQaliteUser.UserID,
                    NotificationTitle = "Reclamation Notification Qalite"
                };
                _context.Notification.Add(notificationQalite);

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var ReceptionReport = new ReceptionReport
                {
                    CreationDate = DateTime.Now,
                    PurchasingReceptionistId = userId,
                    PurchaseQuoteId = PurchaseQuoteId,
                    

                };
                _context.ReceptionReport.Add(ReceptionReport);






                _context.SaveChanges();
            }

            return RedirectToAction(nameof(IndexPurchaseQuote));
        }

        




        [HttpPost]
        public IActionResult CreateserviceNotification(int ServiceQuoteId, string respFinnanceEmail, string respQaliteEmail)
        {
            // Find the user IDs based on the selected emails
            var respFinnanceUser = _context.User.FirstOrDefault(u => u.Email == respFinnanceEmail);
            var respQaliteUser = _context.User.FirstOrDefault(u => u.Email == respQaliteEmail);

            if (respFinnanceUser != null && respQaliteUser != null)
            {
                // Create and save notification for RespServiceFinnance
                var notificationFinnance = new NotificationInfo
                {
                    ServiceQuoteID = ServiceQuoteId,
                    UserID = respFinnanceUser.UserID,
                    NotificationTitle = "Reclamation Notification Finnanciere"
                };
                _context.Notification.Add(notificationFinnance);

                // Create and save notification for RespServiceQalite
                var notificationQalite = new NotificationInfo
                {
                    ServiceQuoteID = ServiceQuoteId,
                    UserID = respQaliteUser.UserID,
                    NotificationTitle = "Reclamation Notification Qalite"
                };
                _context.Notification.Add(notificationQalite);

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var ReceptionReport = new ReceptionReport
                {
                    CreationDate = DateTime.Now,
                    PurchasingReceptionistId = userId,
                    ServiceQuoteId = ServiceQuoteId,
                    

                };
                _context.ReceptionReport.Add(ReceptionReport);






                _context.SaveChanges();
            }

            return RedirectToAction(nameof(IndexPurchaseQuote));
        }



        ////////////////////////////////////////////////////////////////////////////////////


        // GET: PurchasingReceptionists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PurchasingReceptionist == null)
            {
                return NotFound();
            }

            var PurchasingReceptionist = await _context.PurchasingReceptionist
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (PurchasingReceptionist == null)
            {
                return NotFound();
            }

            return View(PurchasingReceptionist);
        }

        // GET: PurchasingReceptionists/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] PurchasingReceptionist PurchasingReceptionist)
        {
            if (ModelState.IsValid)
            {
                // Check if the user with the provided email already exists in the database
                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == PurchasingReceptionist.Email);
                if (existingUser != null)
                {
                    // If user with the same email exists, inform the user with a message
                    ModelState.AddModelError("Email", "User with this email already exists.");
                    return View(PurchasingReceptionist);
                }

                // If the user does not exist, proceed with adding them to the database
                PurchasingReceptionist.Password = HashPassword(PurchasingReceptionist?.Password);
                PurchasingReceptionist.Role = UserRole.Purchasing_receptionist;
                _context.Add(PurchasingReceptionist);
                _context.Add(PurchasingReceptionist);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Users");

            }
            return RedirectToAction("Index", "Users");
        }



        // GET: PurchasingReceptionists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PurchasingReceptionist == null)
            {
                return NotFound();
            }

            var PurchasingReceptionist = await _context.PurchasingReceptionist.FindAsync(id);
            if (PurchasingReceptionist == null)
            {
                return NotFound();
            }
            return View(PurchasingReceptionist);
        }

        // POST: PurchasingReceptionists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,FirstName,LastName,IsAdmin")] PurchasingReceptionist PurchasingReceptionist)
        {
            if (id != PurchasingReceptionist.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == PurchasingReceptionist.Email && u.UserID != id);
                if (existingUser != null)
                {
                    // If user with the same email exists (and has a different ID), inform the user with a message
                    ModelState.AddModelError("Email", "Another user with this email already exists.");
                    return View(PurchasingReceptionist);
                }



                try
                {
                    _context.Update(PurchasingReceptionist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchasingReceptionistExists(PurchasingReceptionist.UserID))
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
            return View(PurchasingReceptionist);
        }

        // GET: PurchasingReceptionists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PurchasingReceptionist == null)
            {
                return NotFound();
            }

            var PurchasingReceptionist = await _context.PurchasingReceptionist
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (PurchasingReceptionist == null)
            {
                return NotFound();
            }

            return View(PurchasingReceptionist);
        }

        // POST: PurchasingReceptionists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PurchasingReceptionist == null)
            {
                return Problem("Entity set 'GAPContext.PurchasingReceptionist'  is null.");
            }
            var PurchasingReceptionist = await _context.PurchasingReceptionist.FindAsync(id);
            if (PurchasingReceptionist != null)
            {
                _context.PurchasingReceptionist.Remove(PurchasingReceptionist);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchasingReceptionistExists(int id)
        {
          return (_context.PurchasingReceptionist?.Any(e => e.UserID == id)).GetValueOrDefault();
        }



        private string HashPassword(string password)
        {
            // use a library like BCrypt or Argon2 to hash the password
            // here's an example using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


       




    }
}
