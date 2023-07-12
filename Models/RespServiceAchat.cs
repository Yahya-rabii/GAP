using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GAP.Models
{
    public class RespServiceAchat : User
    {
        private List<DemandeAchat> demandesAchats;
        private List<Devis> devis;

       

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
