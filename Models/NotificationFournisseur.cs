using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class NotificationFournisseur : Notification
    {
       
        [ForeignKey("Fournisseur")]

        public int? FournisseurID { get; set; } 
        
        [ForeignKey("OffreVente")]

        public int? OffreVenteID { get; set; }




        public NotificationFournisseur() : base() 
        {
            OffreVenteID = 0;
            FournisseurID = 0;
        }
        public NotificationFournisseur(int notificationID, int offreVenteID, string notificationTitle, int fournisseurID) : base(notificationID , notificationTitle)
        {
            OffreVenteID = offreVenteID;
            FournisseurID = fournisseurID; 
        }      
       
    }
}
