using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GAP.Models
{
    public class Fournisseur
    {
        public int FournisseurID { get; set; }

        [Required]
        [StringLength(255)]
        public string? Nom { get; set; }

        [Required]
        [StringLength(255)]
        public string? Email { get; set; }

        [Required]
        [StringLength(255)]
        public string? Password { get; set; }


        [DefaultValue(0)]
        public int NombreTransaction { get; set; } = 0;

        private List<OffreVente> offreVente;

        [DefaultValue(false)]
        public bool IsValid { get; set; } = false;


    }

}
