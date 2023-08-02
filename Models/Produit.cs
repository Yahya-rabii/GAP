using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class Produit
    {
        public int ProduitID { get; set; }
        public float PrixUnitaire { get; set; }
        public float Prixtotal { get; set; }

        public string? Nom { get; set; }
        public int? NombrePiece { get; set; }
        public string? Desc { get; set; }

        [ForeignKey("Fournisseur")]
        public int FournisseurId { get; set; } // Foreign key property

        public Produit() {
        
            PrixUnitaire = 0;
            NombrePiece = 0;
            Nom = string.Empty;
            Desc = string.Empty;
            FournisseurId = 0;
            Prixtotal= 0;


        }
        public Produit(int ID , float prixUnitaire, string nom, int nbrpiece, string desc, int fournisseurId, float prixtotal)
        {
            ProduitID = ID;
            PrixUnitaire = prixUnitaire;
            Nom = nom;
            Desc = desc;
            NombrePiece = nbrpiece;
            FournisseurId = fournisseurId;
            Prixtotal = prixtotal;
        }



    }

}
