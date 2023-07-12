using System.Text.RegularExpressions;

namespace GAP.Models
{
    public class RespServiceFinance : User
    {
        private List<Facture> historiqueFactures;
        private ICollection<Facture> facture;


        public List<Facture> HistoriqueFactures
        {
            get { return historiqueFactures; }
            set { historiqueFactures = value; }
        }

        public ICollection<Facture> Facture
        {
            get { return facture; }
            set { facture = value; }
        }
    }

}
