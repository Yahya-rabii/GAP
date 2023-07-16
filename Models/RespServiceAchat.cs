using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GAP.Models
{
    public class RespServiceAchat 
    {


        public int RespServiceAchatID { get; set; }

        [Required]
        [StringLength(255)]
        public string? Email { get; set; }

        [Required]
        [StringLength(255)]
        public string? Password { get; set; }

        [Required]
        [StringLength(255)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string? LastName { get; set; }


        private List<DemandeAchat> demandesAchats;
        private List<Devis> devis;



        public RespServiceAchat()
        {
            RespServiceAchatID = 0;
            Email = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            demandesAchats = new List<DemandeAchat>();
            devis = new List<Devis>(); 
        }

        public RespServiceAchat(int receptServiceAchatID, string? email, string? password, string? fn, string? ln)
        {
            RespServiceAchatID = receptServiceAchatID;
            Email = email;
            Password = password;
            FirstName = fn;
            LastName = ln;
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
