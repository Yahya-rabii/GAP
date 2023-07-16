using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GAP.Models
{
    public class RespServiceAchat : User
    {


      


        private List<DemandeAchat> demandesAchats;
        private List<Devis> devis;



        public RespServiceAchat() : base()
        {
           
            demandesAchats = new List<DemandeAchat>();
            devis = new List<Devis>(); 
        }

        public RespServiceAchat(string? email, string? password, string? fn, string? ln) : base(email, password, fn, ln)
        {
          
            demandesAchats = new List<DemandeAchat>();
            devis = new List<Devis>();
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
