using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GAP.Models
{
    public class RespServiceAchat : User
    {
        private List<DemandeAchat> demandesAchats;
        private List<Devis> devis;
        public string Discriminator { get; set; } = nameof(ReceptServiceAchat);

        public RespServiceAchat(int userID, string ?email, string ?password, string? firstName, string? lastName, string? tutulaire)
        {
            UserID = userID;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Tutulaire = tutulaire;
            DemandesAchats = new List<DemandeAchat>();
            Devis = new List<Devis>();
        }

        public List<DemandeAchat> DemandesAchats
        {
            get { return demandesAchats; }
            set { demandesAchats = value; }
        }

        public List<Devis> Devis
        {
            get { return devis; }
            set { devis = value; }
        }
    }
}
