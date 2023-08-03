using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
   public class RapportReception
{


        public int RapportReceptionID { get; set; }
        public DateTime DateCreation { get; set; }



        [ForeignKey("ReceptServiceAchat")]
        public int ReceptServiceAchatId { get; set; } // Foreign key property

        
        [ForeignKey("Devis")]
        public int DevisId { get; set; } // Foreign key property
     


        public RapportReception()
        {
            DateCreation = DateTime.Now.Date;
            ReceptServiceAchatId = 0;
            DevisId = 0;

        }
    }

}
