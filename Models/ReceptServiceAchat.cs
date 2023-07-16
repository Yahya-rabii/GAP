using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GAP.Models
{
   

        public class ReceptServiceAchat 
        {

        public int ReceptServiceAchatID { get; set; }

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

        private List<RapportReception> historiqueRapportsReceptions;

        public ReceptServiceAchat()
        {
            ReceptServiceAchatID = 0;
            Email = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            historiqueRapportsReceptions = new List<RapportReception>();
        } 
        
        public ReceptServiceAchat(int receptServiceAchatID,string? email , string? password , string? fn , string? ln)
        {
            ReceptServiceAchatID = receptServiceAchatID;
            Email = email;
            Password = password;
            FirstName = fn;
            LastName = ln;
            historiqueRapportsReceptions = new List<RapportReception>();
        }
        
        public List<RapportReception> HistoriqueRapportsReceptions
            {
                get { return historiqueRapportsReceptions; }
                set { historiqueRapportsReceptions = value; }
            }
        }


    

}
