using System.Text.RegularExpressions;

namespace GAP.Models
{
    public class RespServiceFinance : User
    {
        private List<Facture> historiqueFactures;


        public List<Facture> HistoriqueFactures
        {
            get { return historiqueFactures; }
            set { historiqueFactures = value; }
        }

   
    }

}
