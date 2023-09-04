using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("NotificationAdmin", Title = "NotificationAdmin")]
    public class NotificationAdmin : Notification
    {
        [SwaggerSchema("The unique identifier for the supplier associated with the notification.")]
        [ForeignKey("Supplier")]
        public int? SupplierID { get; set; }

        public NotificationAdmin() : base()
        {
            SupplierID = 0;
        }

        public NotificationAdmin(int notificationID, string notificationTitle, int supplierID) : base(notificationID, notificationTitle)
        {
            SupplierID = supplierID;
        }
    }
}
