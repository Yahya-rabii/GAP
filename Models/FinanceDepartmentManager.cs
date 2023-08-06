using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GAP.Models
{
    public class FinanceDepartmentManager : User
    {
      

        private List<Bill> Billshistory;

        public FinanceDepartmentManager() : base()
        {
           
            Billshistory = new List<Bill>();
        }

        public FinanceDepartmentManager(string? email, string? password, string? fn, string? ln) : base(email, password, fn, ln)
        {
         
            Billshistory = new List<Bill>();
        }



        public List<Bill> HistoriqueBills
        {
            get { return Billshistory; }
            set { Billshistory = value; }
        }

   
    }

}
