using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GAP.Models
{
   

        public class ReceptServiceAchat : User
        {
            private List<RapportReception> historiqueRapportsReceptions;
        public string Discriminator { get; set; } = nameof(ReceptServiceAchat);


        public ReceptServiceAchat(int userID, string? email, string? password, string? firstName, string? lastName, string? tutulaire)
        {
            UserID = userID;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Tutulaire = tutulaire;
            historiqueRapportsReceptions = new List<RapportReception>();
           
        }


        public List<RapportReception> HistoriqueRapportsReceptions
            {
                get { return historiqueRapportsReceptions; }
                set { historiqueRapportsReceptions = value; }
            }
        }


    

}
