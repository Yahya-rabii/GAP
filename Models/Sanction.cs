using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class Sanction
    {

        public int SanctionID { get; set; }
        public string SanctionTitle { get; set;}
        public string SanctionDescription { get;set;}
        [ForeignKey("PurchaseQuoteID")]

        public int PurchaseQuoteID { get; set; }

        [ForeignKey("Supplier")]
        public int? SupplierId { get; set; } // Foreign key property

        public Sanction(int sanctionID, string sanctionTitle, string sanctionDescription, int supplierId)
        {
            SanctionID = sanctionID;
            SanctionTitle = sanctionTitle;
            SanctionDescription = sanctionDescription;
            SupplierId = supplierId;
        }     public Sanction(int sanctionID, string sanctionTitle, string sanctionDescription, int supplierId,int purchaseQuoteID)
        {
            SanctionID = sanctionID;
            SanctionTitle = sanctionTitle;
            SanctionDescription = sanctionDescription;
            SupplierId = supplierId;
            PurchaseQuoteID = purchaseQuoteID;
        }
        public Sanction() {
            SanctionID = 0;
            SanctionTitle = string.Empty;
            SanctionDescription = string.Empty;
            SupplierId = 0;
        }
    }
}
