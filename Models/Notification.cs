using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string NotificationTitle{ get; set; }

        [ForeignKey("User")]

        public int? UserID { get; set; } 
        
        [ForeignKey("DevisID")]

        public int? DevisID { get; set; }




        public Notification()
        {
           NotificationID = 0;
            DevisID = 0;
            NotificationTitle = string.Empty;
        }
        public Notification(int notificationID, int devisID,string notificationTitle)
        {
            DevisID= devisID;
            NotificationID = notificationID;
            NotificationTitle = notificationTitle;
        }
    }
}
