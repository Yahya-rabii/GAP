using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string NotificationTitle{ get; set; }

   


        public Notification()
        {
           NotificationID = 0;
            NotificationTitle = string.Empty;
        }
        public Notification(int notificationID,string notificationTitle)
        {
            NotificationID = notificationID;
            NotificationTitle = notificationTitle;
        }      
 
    }
}
