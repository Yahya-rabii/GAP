namespace GAP.Models
{
    public class Produit
    {
        public int ProduitID { get; set; }
        public float PrixUnitaire { get; set; }

        public string? Nom { get; set; }
        public int? NombrePiece { get; set; }
        public string? Desc { get; set; }


        public Produit() {
        
            PrixUnitaire = 0;
            NombrePiece = 0;
            Nom = string.Empty;
            Desc = string.Empty;


        }
        public Produit(int ID , float prixUnitaire , string nom , int nbrpiece ,string desc)
        {
            ProduitID = ID;
            PrixUnitaire = prixUnitaire; 
            Nom = nom;
            Desc = desc;
            NombrePiece= nbrpiece;
        }


   
    }

}
