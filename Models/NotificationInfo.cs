using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class NotificationInfo : Notification
    {
       
        [ForeignKey("User")]

        public int? UserID { get; set; } 
        
        [ForeignKey("PurchaseQuoteID")]

        public int? PurchaseQuoteID { get; set; }




        public NotificationInfo() : base() 
        {
            PurchaseQuoteID = 0;
            UserID = 0;
        }
        public NotificationInfo(int notificationID, int purchaseQuoteID,string notificationTitle, int Userid) : base(notificationID , notificationTitle)
        {
            PurchaseQuoteID= purchaseQuoteID;
            UserID = Userid; 
        }      
       
    }
}
