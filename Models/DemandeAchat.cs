using System.ComponentModel.DataAnnotations;

namespace GAP.Models
{
    public class DemandeAchat
    {

        public int DemandeAchatID { get; set; }
        public DateTime CreationDate  { get; set; }

        [Required]
        [StringLength(255)]
        public string? Description { get; set; }
        public double Budget { get; set; }
        bool IsValid{ get; set; }
   
    }

}
