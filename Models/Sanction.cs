using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class Sanction
    {

        public int SanctionID { get; set; }
        public string SanctionTitle { get; set;}
        public string SanctionDescription { get;set;}

        [ForeignKey("Fournisseur")]
        public int? FournisseurId { get; set; } // Foreign key property

        public Sanction(int sanctionID, string sanctionTitle, string sanctionDescription, int fournisseurId)
        {
            SanctionID = sanctionID;
            SanctionTitle = sanctionTitle;
            SanctionDescription = sanctionDescription;
            FournisseurId = fournisseurId;
        }
        public Sanction() {
            SanctionID = 0;
            SanctionTitle = string.Empty;
            SanctionDescription = string.Empty;
            FournisseurId = 0;
        }
    }
}
