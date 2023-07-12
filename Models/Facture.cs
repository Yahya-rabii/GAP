namespace GAP.Models
{
    public class Facture
    {
        private int id;
        private Produit produit;
        private Double prix;
        private Fournisseur beneficiaire;
        private bool validite;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public Produit Produit
        {
            get { return produit; }
            set { produit = value; }
        }

        public Double Prix
        {
            get { return prix; }
            set { prix = value; }
        }

        public Fournisseur Beneficiaire
        {
            get { return beneficiaire; }
            set { beneficiaire = value; }
        }

        public bool Validite
        {
            get { return validite; }
            set { validite = value; }
        }
    }

}
