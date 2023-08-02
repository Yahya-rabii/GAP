using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class Sanction
    {

        public int SanctionID { get; set; }
        public string SanctionTitle { get; set;}
        public string SanctionDescription { get;set;}
        [ForeignKey("DevisID")]

        public int DevisID { get; set; }

        [ForeignKey("Fournisseur")]
        public int? FournisseurId { get; set; } // Foreign key property

        public Sanction(int sanctionID, string sanctionTitle, string sanctionDescription, int fournisseurId)
        {
            SanctionID = sanctionID;
            SanctionTitle = sanctionTitle;
            SanctionDescription = sanctionDescription;
            FournisseurId = fournisseurId;
        }     public Sanction(int sanctionID, string sanctionTitle, string sanctionDescription, int fournisseurId,int devisID)
        {
            SanctionID = sanctionID;
            SanctionTitle = sanctionTitle;
            SanctionDescription = sanctionDescription;
            FournisseurId = fournisseurId;
            DevisID = devisID;
        }
        public Sanction() {
            SanctionID = 0;
            SanctionTitle = string.Empty;
            SanctionDescription = string.Empty;
            FournisseurId = 0;
        }
    }
}
