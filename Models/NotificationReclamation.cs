using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class NotificationReclamation : Notification
    {
       
        [ForeignKey("User")]

        public int? UserID { get; set; } 
        
        [ForeignKey("DevisID")]

        public int? DevisID { get; set; }




        public NotificationReclamation() : base() 
        {
            DevisID = 0;
            UserID = 0;
        }
        public NotificationReclamation(int notificationID, int devisID,string notificationTitle, int Userid) : base(notificationID , notificationTitle)
        {
            DevisID= devisID;
            UserID = Userid; 
        }      
       
    }
}
