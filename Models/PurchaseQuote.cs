using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("PurchaseQuote", Title = "Purchase Quote")]
    public class PurchaseQuote
    {
        public int PurchaseQuoteID { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ReceptionDate { get; set; }
        public double? TotalPrice { get; set; }
        public int? typeCntProducts { get; set; }
        public int? SupplierID { get; set; } // Foreign key for Supplier
        public int? PurchasingDepartmentManagerId { get; set; }

        [SwaggerSchema("Product", Title = "Product")]
        public List<Product>? Products { get; set; }

        [SwaggerSchema("Supplier", Title = "Supplier")]
        [ForeignKey("SupplierID")]
        public Supplier? Supplier { get; set; }

        [SwaggerSchema("SaleOffer", Title = "Sale Offer")]
        [ForeignKey("SaleOfferID")]
        public int? SaleOfferID { get; set; }
        public SaleOffer? SaleOffer { get; set; }

        public PurchaseQuote()
        {
            CreationDate = DateTime.Now.Date;
            ReceptionDate = DateTime.Now.Date;
        }
    }
}
