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


        
        public List<RapportReception> HistoriqueRapportsReceptions
            {
                get { return historiqueRapportsReceptions; }
                set { historiqueRapportsReceptions = value; }
            }
        }


    

}
