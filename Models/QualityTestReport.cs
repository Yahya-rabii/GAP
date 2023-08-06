using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class QualityTestReport
    {
        public int QualityTestReportID { get; set; }
        public bool StateValidity { get; set; }
        public bool CntItemsValidity { get; set; }
        public bool OperationValidity { get; set; }
                                                      
        public  int? QualityTestingDepartmentManagerId { get; set; } // Navigation property

        public int PurchaseQuoteId { get; set; } // Foreign key property


    }
}
