using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("QualityTestingDepartmentManager", Title = "Quality Testing Department Manager")]
    public class QualityTestingDepartmentManager : User
    {
        private List<QualityTestReport> QualityTestReportHistory;

        public QualityTestingDepartmentManager() : base()
        {
            QualityTestReportHistory = new List<QualityTestReport>();
        }

        public QualityTestingDepartmentManager(string email, string password, string fn, string ln) : base(email, password, fn, ln)
        {
            QualityTestReportHistory = new List<QualityTestReport>();
        }

        [SwaggerSchema("Quality Test Report History", Title = "Quality Test Report History")]
        public List<QualityTestReport> HistoriqueRapportQualite
        {
            get { return QualityTestReportHistory; }
            set { QualityTestReportHistory = value; }
        }
    }
}
