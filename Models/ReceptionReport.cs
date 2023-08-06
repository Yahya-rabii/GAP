using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
   public class ReceptionReport
{


        public int ReceptionReportID { get; set; }
        public DateTime CreationDate { get; set; }



        [ForeignKey("PurchasingReceptionist")]
        public int PurchasingReceptionistId { get; set; } // Foreign key property

        
        [ForeignKey("PurchaseQuote")]
        public int PurchaseQuoteId { get; set; } // Foreign key property
     


        public ReceptionReport()
        {
            CreationDate = DateTime.Now.Date;
            PurchasingReceptionistId = 0;
            PurchaseQuoteId = 0;

        }
    }

}
