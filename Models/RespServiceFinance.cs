using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GAP.Models
{
    public class RespServiceFinance : User
    {
      

        private List<Facture> historiqueFactures;

        public RespServiceFinance() : base()
        {
           
            historiqueFactures = new List<Facture>();
        }

        public RespServiceFinance(string? email, string? password, string? fn, string? ln) : base(email, password, fn, ln)
        {
         
            historiqueFactures = new List<Facture>();
        }



        public List<Facture> HistoriqueFactures
        {
            get { return historiqueFactures; }
            set { historiqueFactures = value; }
        }

   
    }

}
