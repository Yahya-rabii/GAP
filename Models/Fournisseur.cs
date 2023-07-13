using System.ComponentModel.DataAnnotations;

namespace GAP.Models
{
    public class Fournisseur
    {
        private int id;
        private int nom;
        [Required]
        [StringLength(255)]
        public string? Email { get; set; }

        [Required]
        [StringLength(255)]
        public string? Password { get; set; }
        private int nombreTransaction;
        private List<OffreVente> offreVente;

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

        public List<OffreVente> OffreVente
        {
            get { return offreVente; }
            set { offreVente = value; }
        }

      
    }

}
