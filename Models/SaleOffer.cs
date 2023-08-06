using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class SaleOffer
    {
        public int SaleOfferID { get; set; }
        public Supplier? Supplier { get; set; }
        public double UnitProfit { get; set; }
        public double TotalProfit { get; set; }
        public List<Product> Products { get; set; }
        public bool Validity { get; set; }

          [ForeignKey("Supplier")]
        public int SupplierId { get; set; } // Foreign key property

        [NotMapped] // Exclude this property from database mapping
        public int SelectedProductId { get; set; }

        [ForeignKey("PurchaseRequest")]
        public int PurchaseRequestId { get; set; } // Foreign key property

        public PurchaseRequest? PurchaseRequest { get; set; }



    }

}
