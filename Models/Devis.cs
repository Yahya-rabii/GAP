namespace GAP.Models
{
    public class Devis
    {
        private int id;
        private DateTime dateCreation;
        private Produit produit;
        private double prixTTL;
        private DateTime dateReception;
        private Fournisseur fournisseur;
        private int nombrePiece;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public DateTime DateCreation
        {
            get { return dateCreation; }
            set { dateCreation = value; }
        }

        public Produit Produit
        {
            get { return produit; }
            set { produit = value; }
        }

        public double PrixTTL
        {
            get { return prixTTL; }
            set { prixTTL = value; }
        }

        public DateTime DateReception
        {
            get { return dateReception; }
            set { dateReception = value; }
        }

        public Fournisseur Fournisseur
        {
            get { return fournisseur; }
            set { fournisseur = value; }
        }

        public int NombrePiece
        {
            get { return nombrePiece; }
            set { nombrePiece = value; }
        }
    }

}
