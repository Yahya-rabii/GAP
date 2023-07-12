namespace GAP.Models
{
    public class RapportTestQualite
    {
        private int id;
        private bool validiteEtat;
        private bool validiteNbrPiece;
        private bool vaiditeFonctionnement;


        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public bool ValiditeEtat
        {
            get { return validiteEtat; }
            set { validiteEtat = value; }
        }

        public bool ValiditeNbrPiece
        {
            get { return validiteNbrPiece; }
            set { validiteNbrPiece = value; }
        }

        public bool ValiditeFonctionnement
        {
            get { return vaiditeFonctionnement; }
            set { vaiditeFonctionnement = value; }
        }
    }

}
