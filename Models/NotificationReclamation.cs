using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("NotificationReclamation", Title = "NotificationReclamation")]
    public class NotificationReclamation : Notification
    {
        public NotificationReclamation() : base()
        {
        }

        public NotificationReclamation(int notificationID, string notificationTitle, int supplierID) : base(notificationID, notificationTitle)
        {
        }
    }
}
