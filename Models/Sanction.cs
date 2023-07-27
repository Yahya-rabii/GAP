using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class Sanction
    {

        public int SanctionID { get; set; }
        public string SanctionTitle { get; set;}
        public string SanctionDescription { get;set;}

        [ForeignKey("Fournisseur")]
        public int FournisseurId { get; set; } // Foreign key property


    }
}
