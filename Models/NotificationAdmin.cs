using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class NotificationAdmin : Notification
    {

        [ForeignKey("Supplier")]

        public int? SupplierID { get; set; } 
        





        public NotificationAdmin() : base()
        {
         SupplierID = 0;
        }
        public NotificationAdmin(int notificationID,string notificationTitle, int Supplierid) : base(notificationID,notificationTitle)
        {
          SupplierID=Supplierid;
        }      
     
    }
}
