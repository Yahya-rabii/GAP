using GAP.Helper;

namespace GAP.Models
{
    public class OffreVente
    {
        private int id;
        private Fournisseur provider;
        private double prixTTL;
        private List<Produit> produit;
        private Desc description;
        private bool validite;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public Fournisseur Provider
        {
            get { return provider; }
            set { provider = value; }
        }

        public double PrixTTL
        {
            get { return prixTTL; }
            set { prixTTL = value; }
        }

        public List<Produit> Produit
        {
            get { return produit; }
            set { produit = value; }
        }

        public Desc Description
        {
            get { return description; }
            set { description = value; }
        }

        public bool Validite
        {
            get { return validite; }
            set { validite = value; }
        }
    }

}
