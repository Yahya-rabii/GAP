namespace GAP.Models
{
    public class RespServiceQualite : User
    {
        private List<RapportTestQualite> historiqueRapportQualite;

        public List<RapportTestQualite> HistoriqueRapportQualite
        {
            get { return historiqueRapportQualite; }
            set { historiqueRapportQualite = value; }
        }

    }

}
