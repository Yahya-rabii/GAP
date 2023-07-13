using System.Text.RegularExpressions;

namespace GAP.Models
{
    public class RespServiceFinance : User
    {
        private List<Facture> historiqueFactures;

        public string Discriminator { get; set; } = nameof(RespServiceFinance);


        public RespServiceFinance(int userID, string? email, string? password, string? firstName, string? lastName, string? tutulaire)
        {
            UserID = userID;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Tutulaire = tutulaire;
            historiqueFactures = new List<Facture>() ;
        }

        public List<Facture> HistoriqueFactures
        {
            get { return historiqueFactures; }
            set { historiqueFactures = value; }
        }

   
    }

}
