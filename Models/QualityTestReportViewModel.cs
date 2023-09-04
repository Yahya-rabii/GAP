using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("QualityTestReportViewModel", Title = "Quality Test Report View Model")]
    public class QualityTestReportViewModel
    {
        [SwaggerSchema("Quality Test Report", Title = "Quality Test Report")]
        public QualityTestReport QualityTestReport { get; set; }

        [SwaggerSchema("Purchase Quote", Title = "Purchase Quote")]
        public PurchaseQuote PurchaseQuote { get; set; }
    }
}
