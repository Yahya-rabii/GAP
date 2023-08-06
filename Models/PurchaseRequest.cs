using System.ComponentModel.DataAnnotations;

namespace GAP.Models
{
    public class PurchaseRequest
    {

        public int PurchaseRequestID { get; set; }
        public DateTime CreationDate  { get; set; }

        [Required]
        [StringLength(255)]
        public string? Description { get; set; }
        public double Budget { get; set; }
        public bool IsValid { get; set; } = true;


        public PurchaseRequest()
        {
            CreationDate = DateTime.Now.Date; 
        }

    }

}
