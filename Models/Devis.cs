using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class Devis
    {
        public int DevisID { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateReception { get; set; }
        public int? ProduitID { get; set; } // Foreign key for Product
        public double? PrixTTL { get; set; }
        public int NombrePiece { get; set; }
        public int? FournisseurID { get; set; } // Foreign key for Fournisseur
        public int? RespServiceAchatId { get; set; }

        [ForeignKey("ProduitID")]
        public Produit? Produit { get; set; }

        [ForeignKey("FournisseurID")]
        public Fournisseur? Fournisseur { get; set; }
    }
}
