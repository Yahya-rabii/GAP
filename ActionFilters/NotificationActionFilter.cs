﻿using System.Linq;
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

                if (!context.HttpContext.User.IsInRole("Admin"))
                {
                    // Fetch notifications for the authenticated user
                    var notifications = await _context.NotificationReclamation
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
                    var notificationsAdmin = await _context.NotificationAdmin
                        .ToListAsync();

                    // Get the total number of admin notifications
                    int totalNotificationsAdmin = notificationsAdmin.Count;

                    // Fetch the profile picture of the admin user
                    var admin = await _context.User.FirstOrDefaultAsync(u => u.UserID == userId);
                    byte[] UserProfilePicture = admin?.ProfilePicture;
                    var UserEmail = admin?.Email;
                    var UserID = admin?.UserID;

                    // Pass the admin notifications, total number, and profile picture to the view
                    context.HttpContext.Items["NotificationsAdmin"] = notificationsAdmin;
                    context.HttpContext.Items["TotalNotificationsAdmin"] = totalNotificationsAdmin;
                    context.HttpContext.Items["UserProfilePicture"] = UserProfilePicture;
                    context.HttpContext.Items["UserEmail"] = UserEmail;
                    context.HttpContext.Items["UserID"] = UserID;

                }
            }

            await next();
        }
    }
}
