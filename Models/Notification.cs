using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("Notification", Title = "Notification")]
    public class Notification
    {
        [SwaggerSchema("The unique identifier for the notification.")]
        public int NotificationID { get; set; }

        [SwaggerSchema("The title of the notification.")]
        public string NotificationTitle { get; set; }

        public Notification()
        {
            NotificationID = 0;
            NotificationTitle = string.Empty;
        }

        public Notification(int notificationID, string notificationTitle)
        {
            NotificationID = notificationID;
            NotificationTitle = notificationTitle;
        }
    }
}
