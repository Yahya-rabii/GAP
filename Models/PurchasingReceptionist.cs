using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GAP.Models
{
   

        public class PurchasingReceptionist : User
    {

       

        private List<ReceptionReport> ReceptionReportHistory;

        public PurchasingReceptionist() :base() 
        {
            
            ReceptionReportHistory = new List<ReceptionReport>();
        } 
        
        public PurchasingReceptionist(string? email , string? password , string? fn , string? ln) : base(email, password, fn, ln)
        {
           
            ReceptionReportHistory = new List<ReceptionReport>();
        }
        
        public List<ReceptionReport> HistoriqueRapportsReceptions
            {
                get { return ReceptionReportHistory; }
                set { ReceptionReportHistory = value; }
            }
        }


    

}
