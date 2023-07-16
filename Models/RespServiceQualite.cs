using System.ComponentModel.DataAnnotations;

namespace GAP.Models
{
    public class RespServiceQualite : User
    {

    

        private List<RapportTestQualite> historiqueRapportQualite;

        public RespServiceQualite() : base()
        {
          
            historiqueRapportQualite= new List<RapportTestQualite>();
        }

        public RespServiceQualite(string email, string password, string fn, string ln) : base(email, password, fn, ln)
        {
            historiqueRapportQualite = new List<RapportTestQualite>();
        }


        public List<RapportTestQualite> HistoriqueRapportQualite
        {
            get { return historiqueRapportQualite; }
            set { historiqueRapportQualite = value; }
        }

    }

}
