using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GAP.Models
{
    public class Admin
    {
        public int AdminID { get; set; }

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

        [DefaultValue(true)]
        public bool IsAdmin { get; set; } = true;
    }

   
}
