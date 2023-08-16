using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using GAP.Models;
using GAP.Data;

namespace GAP.ActionFilters
{
    public class NotificationActionFilter : IAsyncActionFilter
    {
        private readonly GAPContext _context;

        public NotificationActionFilter(GAPContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


                if (context.HttpContext.User.IsInRole("Admin"))
                {

                    // from table ReclamationsHistory get all reclamations ids and count them
                    var reclamationIds = await _context.ReclamationsHistory
                                                    .Select(r => r.ReclamationsID)
                                                    .ToListAsync();
                    int totalReclamations = reclamationIds.Count;

                    // delete the reclamations history
                    _context.ReclamationsHistory.RemoveRange(_context.ReclamationsHistory);
                    _context.SaveChanges();


                    if (totalReclamations > 0)
                    {
                        var notificationReclamation = new NotificationReclamation();
                        notificationReclamation.NotificationTitle = totalReclamations + " New Reclamation Has Landded ";
                        
                        _context.NotificationReclamation.Add(notificationReclamation);
                        _context.SaveChanges() ;
                    }

                    var notificationsReclamation = await _context.NotificationReclamation
                        .ToListAsync();
                    int TotalnotificationsReclamation = notificationsReclamation.Count;

                    _context.NotificationReclamation.RemoveRange(notificationsReclamation);
                    _context .SaveChanges() ;
                    // Fetch admin notifications
                    var notificationsAdmin = await _context.NotificationAdmin
                        .ToListAsync();

                    // Get the total number of admin notifications
                    int totalNotificationsAdmin = notificationsAdmin.Count + TotalnotificationsReclamation;

                    // Fetch the profile picture of the admin user
                    var admin = await _context.User.FirstOrDefaultAsync(u => u.UserID == userId);
                    byte[] UserProfilePicture = admin?.ProfilePicture;
                    var UserEmail = admin?.Email;
                    var UserID = admin?.UserID;

                    // Pass the admin notifications, total number, and profile picture to the view
                    context.HttpContext.Items["NotificationsAdmin"] = notificationsAdmin;
                    context.HttpContext.Items["notificationsReclamation"] = notificationsReclamation;
                    context.HttpContext.Items["TotalnotificationsReclamation"] = TotalnotificationsReclamation;
                    context.HttpContext.Items["TotalNotificationsAdmin"] = totalNotificationsAdmin;
                    context.HttpContext.Items["UserProfilePicture"] = UserProfilePicture;
                    context.HttpContext.Items["UserEmail"] = UserEmail;
                    context.HttpContext.Items["UserID"] = UserID;

                }


                else if (!context.HttpContext.User.IsInRole("Admin") && !context.HttpContext.User.IsInRole("Supplier"))
                {
                    // Fetch notifications for the authenticated user
                    var notifications = await _context.NotificationInfo
                        .Where(n => n.UserID == userId)
                        .ToListAsync();

                    // Get the total number of notifications
                    int totalNotifications = notifications.Count;

                    // Fetch the profile picture of the user
                    var user = await _context.User.FirstOrDefaultAsync(u => u.UserID == userId);
                    byte[] profilePicture = user?.ProfilePicture;
                    var UserEmail = user?.Email;
                    var UserID = user?.UserID;

                    // Pass the notifications, total number, and profile picture to the view
                    context.HttpContext.Items["Notifications"] = notifications;
                    context.HttpContext.Items["TotalNotifications"] = totalNotifications;
                    context.HttpContext.Items["UserProfilePicture"] = profilePicture;
                    context.HttpContext.Items["UserEmail"] = UserEmail;
                    context.HttpContext.Items["UserID"] = UserID;
                }
                 

                else
                {

                    // Fetch admin notifications
                    var notificationsSupplier = await _context.NotificationSupplier
                        .ToListAsync();

                    // Get the total number of admin notifications
                    int totalNotificationsSupplier = notificationsSupplier.Count;

                    // Fetch the profile picture of the admin user
                    var Supplier = await _context.User.Where(u=>u.Role == UserRole.Supplier).FirstOrDefaultAsync(u => u.UserID == userId);
                    byte[] SupplierProfilePicture = Supplier?.ProfilePicture;
                    var SupplierEmail = Supplier?.Email;
                    var SupplierID = Supplier?.UserID;

                    // Pass the admin notifications, total number, and profile picture to the view
                    context.HttpContext.Items["NotificationSupplier"] = notificationsSupplier;
                    context.HttpContext.Items["TotalNotificationsSupplier"] = totalNotificationsSupplier;
                    context.HttpContext.Items["SupplierProfilePicture"] = SupplierProfilePicture;
                    context.HttpContext.Items["SupplierEmail"] = SupplierEmail;
                    context.HttpContext.Items["SupplierID"] = SupplierID;


                }
            }

            await next();
        }
    }
}
