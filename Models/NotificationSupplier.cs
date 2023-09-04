using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("NotificationSupplier", Title = "NotificationSupplier")]
    public class NotificationSupplier : Notification
    {
        [ForeignKey("Supplier")]
        public int? SupplierID { get; set; }

        [ForeignKey("SaleOffer")]
        public int? SaleOfferID { get; set; }     
        
        [ForeignKey("ServiceOffer")]
        public int? ServiceOfferID { get; set; }

        public NotificationSupplier() : base()
        {
            SaleOfferID = 0;
            ServiceOfferID = 0;
            SupplierID = 0;
        }

        public NotificationSupplier(int notificationID, int saleOfferID, string notificationTitle, int supplierID , int serviceOffreID) : base(notificationID, notificationTitle)
        {
            SaleOfferID = saleOfferID;
            ServiceOfferID= serviceOffreID;
            SupplierID = supplierID;
        }
    }
}
