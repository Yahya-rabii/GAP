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
        public bool IsValid { get; set; } = true;


        public DemandeAchat()
        {
            CreationDate = DateTime.Now.Date; 
        }

    }

}
