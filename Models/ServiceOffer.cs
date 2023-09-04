using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("Service Offer", Title = "Service Offer")]
    public class ServiceOffer
    {
        [SwaggerSchema("Service Offer ID", Description = "The ID of the Service Offer.")]
        public int ServiceOfferID { get; set; }

        [SwaggerSchema("Supplier", Description = "The supplier associated with the Service offer.")]
        public Supplier? Supplier { get; set; }



        [SwaggerSchema("Total Profit", Description = "The total profit of the Service offer.")]
        public double Price { get; set; }

   
        [SwaggerSchema("Validity", Description = "The validity status of the Service offer.")]
        public bool Validity { get; set; }

        [ForeignKey("Supplier")]
        [SwaggerSchema("Supplier ID", Description = "The ID of the supplier associated with the Service offer.")]
        public int SupplierId { get; set; } // Foreign key property


        [ForeignKey("ServiceRequest")]
        [SwaggerSchema("Service Request ID", Description = "The ID of the Service request associated with the sale offer.")]
        public int ServiceRequestId { get; set; } // Foreign key property

        [SwaggerSchema("Service Request", Description = "The Service request associated with the sale offer.")]
        public ServiceRequest? ServiceRequest { get; set; }
    }
}
