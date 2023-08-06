using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class Bill
    {
        public int BillID { get; set; }
        public bool Validity { get; set; }
        public int FinanceDepartmentManagerId { get; set; }
        [ForeignKey("PurchaseQuoteID")]
        public int? PurchaseQuoteID { get; set; }
    }
}