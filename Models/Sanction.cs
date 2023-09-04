using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("Sanction", Title = "Sanction")]
    public class Sanction
    {
        [SwaggerSchema("SanctionID", Description = "The unique identifier for the sanction.")]
        public int SanctionID { get; set; }

        [SwaggerSchema("Sanction Title", Description = "The title of the sanction.")]
        public string SanctionTitle { get; set; }

        [SwaggerSchema("Sanction Description", Description = "The description of the sanction.")]
        public string SanctionDescription { get; set; }

        [ForeignKey("PurchaseQuoteID")]
        [SwaggerSchema("Purchase Quote ID", Description = "The ID of the associated purchase quote.")]
        public int PurchaseQuoteID { get; set; }

        [ForeignKey("Supplier")]
        [SwaggerSchema("Supplier ID", Description = "The ID of the associated supplier.")]
        public int? SupplierId { get; set; } // Foreign key property

        public Sanction(int sanctionID, string sanctionTitle, string sanctionDescription, int supplierId)
        {
            SanctionID = sanctionID;
            SanctionTitle = sanctionTitle;
            SanctionDescription = sanctionDescription;
            SupplierId = supplierId;
        }

        public Sanction(int sanctionID, string sanctionTitle, string sanctionDescription, int supplierId, int purchaseQuoteID)
        {
            SanctionID = sanctionID;
            SanctionTitle = sanctionTitle;
            SanctionDescription = sanctionDescription;
            SupplierId = supplierId;
            PurchaseQuoteID = purchaseQuoteID;
        }

        public Sanction()
        {
            SanctionID = 0;
            SanctionTitle = string.Empty;
            SanctionDescription = string.Empty;
            SupplierId = 0;
        }
    }
}
