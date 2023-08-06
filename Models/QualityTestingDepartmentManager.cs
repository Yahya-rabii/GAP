using System.ComponentModel.DataAnnotations;

namespace GAP.Models
{
    public class QualityTestingDepartmentManager : User
    {

    

        private List<QualityTestReport> QualityTestReportHistory;

        public QualityTestingDepartmentManager() : base()
        {
            QualityTestReportHistory= new List<QualityTestReport>();
        }

        public QualityTestingDepartmentManager(string email, string password, string fn, string ln) : base(email, password, fn, ln)
        {
            QualityTestReportHistory = new List<QualityTestReport>();
        }


        public List<QualityTestReport> HistoriqueRapportQualite
        {
            get { return QualityTestReportHistory; }
            set { QualityTestReportHistory = value; }
        }
   

    }

}
