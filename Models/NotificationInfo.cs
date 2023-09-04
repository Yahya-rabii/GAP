using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("NotificationInfo", Title = "NotificationInfo")]
    public class NotificationInfo : Notification
    {
        [SwaggerSchema("The unique identifier for the user associated with the notification.")]
        [ForeignKey("User")]
        public int? UserID { get; set; }

        [SwaggerSchema("The unique identifier for the purchase quote associated with the notification.")]
        [ForeignKey("PurchaseQuoteID")]
        public int? PurchaseQuoteID { get; set; }   
        
        
        [SwaggerSchema("The unique identifier for the Service quote associated with the notification.")]
        [ForeignKey("ServiceQuoteID")]
        public int? ServiceQuoteID { get; set; }

        public NotificationInfo() : base()
        {
            PurchaseQuoteID = 0;
            ServiceQuoteID = 0;
            UserID = 0;
        }

        public NotificationInfo(int notificationID, int purchaseQuoteID,int serviceQuoteID, string notificationTitle, int userID) : base(notificationID, notificationTitle)
        {
            PurchaseQuoteID = purchaseQuoteID;
            ServiceQuoteID=serviceQuoteID;
            UserID = userID;

        }
    }
}
