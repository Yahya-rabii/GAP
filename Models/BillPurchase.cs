using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("BillPurchase", Title = "BillPurchase")]
    public class BillPurchase:Bill
    {
      

      
        [ForeignKey("PurchaseQuote")]
        [SwaggerSchema("ID of the associated Purchase Quote")]
        public int PurchaseQuoteID { get; set; }



        public BillPurchase() : base()
        {

            PurchaseQuoteID = 0;

        }

        public BillPurchase( int billID, bool validity, int financeDepartmentManagerId, int purchaseQuoteID) : base(billID, validity, financeDepartmentManagerId)
        {
            PurchaseQuoteID = purchaseQuoteID;
        }


    }
}
