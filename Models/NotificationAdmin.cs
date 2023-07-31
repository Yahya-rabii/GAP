using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class NotificationAdmin : Notification
    {

        [ForeignKey("Fournisseur")]

        public int? FournisseurID { get; set; } 
        





        public NotificationAdmin() : base()
        {
         FournisseurID = 0;
        }
        public NotificationAdmin(int notificationID,string notificationTitle, int Fournisseurid) : base(notificationID,notificationTitle)
        {
          FournisseurID=Fournisseurid;
        }      
     
    }
}
