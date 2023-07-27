using GAP.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Add this namespace for ToListAsync()

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
                // Fetch notifications for the authenticated user
                var userId = int.Parse(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var notifications = await _context.Notification
                    .Where(n => n.UserID == userId)
                    .ToListAsync();

                // Get the total number of notifications
                int totalNotifications = notifications.Count;

                // Pass the notifications and total number to the view
                context.HttpContext.Items["Notifications"] = notifications;
                context.HttpContext.Items["TotalNotifications"] = totalNotifications;
            }

            await next();
        }
    }
}
