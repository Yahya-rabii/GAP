using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class Devis
    {
        public int DevisID { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateReception { get; set; }
        public double? PrixTTL { get; set; }
        public int? Ntypeproduits { get; set; }
        public int? FournisseurID { get; set; } // Foreign key for Fournisseur
        public int? RespServiceAchatId { get; set; }

        public List<Produit>? Produits { get; set; }

        [ForeignKey("FournisseurID")]
 
        public Fournisseur? Fournisseur { get; set; }



        [ForeignKey("OffreVenteID")]
        public int? OffreVenteID { get; set; }
        public OffreVente? OffreVente { get; set; }


        public Devis()
        {
            DateCreation = DateTime.Now.Date;
            DateReception = DateTime.Now.Date; 

        }


    }
}
