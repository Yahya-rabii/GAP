using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class OffreVente
    {
        public int OffreVenteID { get; set; }
        public Fournisseur? Fournisseur { get; set; }
        public double PrixTTL { get; set; }
        public List<Produit> Produits { get; set; }
        public bool Validite { get; set; }

          [ForeignKey("Fournisseur")]
        public int FournisseurId { get; set; } // Foreign key property

        [NotMapped] // Exclude this property from database mapping
        public int SelectedProduitId { get; set; }

        [ForeignKey("DemandeAchat")]
        public int DemandeAchatId { get; set; } // Foreign key property

        public DemandeAchat? DemandeAchat { get; set; }



    }

}
