using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class PurchaseQuote
    {
        public int PurchaseQuoteID { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ReceptionDate { get; set; }
        public double? TotalPrice { get; set; }
        public int? typeCntProducts { get; set; }
        public int? SupplierID { get; set; } // Foreign key for Supplier
        public int? PurchasingDepartmentManagerId { get; set; }

        public List<Product>? Products { get; set; }

        [ForeignKey("SupplierID")]
 
        public Supplier? Supplier { get; set; }



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
