using System;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("ReceptionReport", Title = "Reception Report")]
    public class ReceptionReport
    {
        [SwaggerSchema("Reception Report ID", Description = "The ID of the reception report.")]
        public int ReceptionReportID { get; set; }

        [SwaggerSchema("Creation Date", Description = "The date when the reception report was created.")]
        public DateTime CreationDate { get; set; }

        [ForeignKey("PurchasingReceptionist")]
        [SwaggerSchema("Purchasing Receptionist ID", Description = "The ID of the purchasing receptionist associated with the report.")]
        public int PurchasingReceptionistId { get; set; } // Foreign key property

        [ForeignKey("PurchaseQuote")]
        [SwaggerSchema("Purchase Quote ID", Description = "The ID of the purchase quote associated with the report.")]
        public int PurchaseQuoteId { get; set; } // Foreign key property
                                                 
        
        [ForeignKey("ServiceQuote")]
        [SwaggerSchema("Service Quote ID", Description = "The ID of the Service quote associated with the report.")]
        public int ServiceQuoteId { get; set; } // Foreign key property

        public ReceptionReport()
        {
            CreationDate = DateTime.Now.Date;
            PurchasingReceptionistId = 0;
            PurchaseQuoteId = 0;
        }
    }
}
