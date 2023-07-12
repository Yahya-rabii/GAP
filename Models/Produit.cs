using GAP.Helper;

namespace GAP.Models
{
    public class Produit
    {
        private int id;
        private int prixUnitaire;
        private int prixTotal;
        private Desc description;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int PrixUnitaire
        {
            get { return prixUnitaire; }
            set { prixUnitaire = value; }
        }

        public int PrixTotal
        {
            get { return prixTotal; }
            set { prixTotal = value; }
        }

        public Desc Description
        {
            get { return description; }
            set { description = value; }
        }
    }

}
