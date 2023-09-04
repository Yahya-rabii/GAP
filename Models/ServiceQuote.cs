using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("PurchaseQuote", Title = "Purchase Quote")]
    public class ServiceQuote
    {
        public int ServiceQuoteID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double? Price { get; set; }
        public int? SupplierID { get; set; } // Foreign key for Supplier
        public int? PurchasingDepartmentManagerId { get; set; }

        

        [SwaggerSchema("Supplier", Title = "Supplier")]
        [ForeignKey("SupplierID")]
        public Supplier? Supplier { get; set; }

        [SwaggerSchema("ServiceOffer", Title = "Service Offer")]
        [ForeignKey("ServiceOfferID")]
        public int? ServiceOfferID { get; set; }
        public ServiceOffer? ServiceOffer { get; set; }

        public ServiceQuote()
        {
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
        }
    }
}
