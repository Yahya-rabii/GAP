using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class NotificationReclamation : Notification
    {


        
        public NotificationReclamation() : base()
        {
        }
        public NotificationReclamation(int notificationID,string notificationTitle, int Supplierid) : base(notificationID,notificationTitle)
        {
        }      
     
    }
}
