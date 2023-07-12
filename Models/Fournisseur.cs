namespace GAP.Models
{
    public class Fournisseur
    {
        private int id;
        private int nom;
        private int nombreTransaction;
        private ICollection<OffreVente> offreVente;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public int NombreTransaction
        {
            get { return nombreTransaction; }
            set { nombreTransaction = value; }
        }

        public ICollection<OffreVente> OffreVente
        {
            get { return offreVente; }
            set { offreVente = value; }
        }

        public Fournisseur()
        {
            offreVente = new HashSet<OffreVente>();
        }

        public void AddOffreVente(OffreVente newOffreVente)
        {
            if (newOffreVente != null && !offreVente.Contains(newOffreVente))
                offreVente.Add(newOffreVente);
        }

        public void RemoveOffreVente(OffreVente oldOffreVente)
        {
            if (oldOffreVente != null && offreVente.Contains(oldOffreVente))
                offreVente.Remove(oldOffreVente);
        }

        public void RemoveAllOffreVente()
        {
            offreVente.Clear();
        }
    }

}
