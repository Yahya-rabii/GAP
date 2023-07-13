namespace GAP.Models
{
    public class RespServiceQualite : User
    {
        private List<RapportTestQualite> historiqueRapportQualite;

        public string Discriminator { get; set; } = nameof(RespServiceQualite);


        public RespServiceQualite(int userID, string? email, string? password, string? firstName, string? lastName, string? tutulaire)
                : base(userID, email, password, firstName, lastName, tutulaire)

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
