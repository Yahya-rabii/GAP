namespace GAP.Models
{
    public class RespServiceQualite : User
    {
        private List<RapportTestQualite> historiqueRapportQualite;
        private ICollection<RapportTestQualite> rapportTestQualite;

        public List<RapportTestQualite> HistoriqueRapportQualite
        {
            get { return historiqueRapportQualite; }
            set { historiqueRapportQualite = value; }
        }

        public ICollection<RapportTestQualite> RapportTestQualite
        {
            get { return rapportTestQualite; }
            set { rapportTestQualite = value; }
        }
    }

}
