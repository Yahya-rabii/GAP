using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("QualityTestReport", Title = "Quality Test Report")]
    public class QualityTestReport
    {
        [SwaggerSchema("Quality Test Report ID")]
        public int QualityTestReportID { get; set; }

        [SwaggerSchema("State Validity")]
        public bool StateValidity { get; set; }

        [SwaggerSchema("Count Items Validity")]
        public bool CntItemsValidity { get; set; }

        [SwaggerSchema("Operation Validity")]
        public bool OperationValidity { get; set; }

        [SwaggerSchema("Quality Testing Department Manager ID", Title = "Navigation Property")]
        public int? QualityTestingDepartmentManagerId { get; set; }

        [SwaggerSchema("Purchase Quote ID", Title = "Foreign Key Property")]
        public int PurchaseQuoteId { get; set; }      
        
        [SwaggerSchema("Service Quote ID", Title = "Foreign Key Property")]
        public int ServiceQuoteId { get; set; }
    }
}
