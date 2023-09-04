using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("Sale Offer", Title = "Sale Offer")]
    public class SaleOffer
    {
        [SwaggerSchema("Sale Offer ID", Description = "The ID of the sale offer.")]
        public int SaleOfferID { get; set; }

        [SwaggerSchema("Supplier", Description = "The supplier associated with the sale offer.")]
        public Supplier? Supplier { get; set; }

        [SwaggerSchema("Unit Profit", Description = "The unit profit of the sale offer.")]
        public double UnitProfit { get; set; }

        [SwaggerSchema("Total Profit", Description = "The total profit of the sale offer.")]
        public double TotalProfit { get; set; }

        [SwaggerSchema("Products", Description = "The list of products associated with the sale offer.")]
        public List<Product> Products { get; set; }

        [SwaggerSchema("Validity", Description = "The validity status of the sale offer.")]
        public bool Validity { get; set; }

        [ForeignKey("Supplier")]
        [SwaggerSchema("Supplier ID", Description = "The ID of the supplier associated with the sale offer.")]
        public int SupplierId { get; set; } // Foreign key property

        [NotMapped]
        [SwaggerSchema("Selected Product ID", Description = "The ID of the selected product (excluded from database mapping).")]
        public int SelectedProductId { get; set; }

        [ForeignKey("PurchaseRequest")]
        [SwaggerSchema("Purchase Request ID", Description = "The ID of the purchase request associated with the sale offer.")]
        public int PurchaseRequestId { get; set; } // Foreign key property

        [SwaggerSchema("Purchase Request", Description = "The purchase request associated with the sale offer.")]
        public PurchaseRequest? PurchaseRequest { get; set; }
    }
}
