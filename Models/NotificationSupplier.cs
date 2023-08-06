using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class NotificationSupplier : Notification
    {
       
        [ForeignKey("Supplier")]

        public int? SupplierID { get; set; } 
        
        [ForeignKey("SaleOffer")]

        public int? SaleOfferID { get; set; }




        public NotificationSupplier() : base() 
        {
            SaleOfferID = 0;
            SupplierID = 0;
        }
        public NotificationSupplier(int notificationID, int saleOfferID, string notificationTitle, int supplierID) : base(notificationID , notificationTitle)
        {
            SaleOfferID = saleOfferID;
            SupplierID = supplierID; 
        }      
       
    }
}
