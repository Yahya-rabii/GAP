using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class OrdreCreationFacture
    {

        public int OrdreCreationFactureID { get; set; }

        [ForeignKey("DevisID")]
        public int? DevisID { get; set; }
        public Devis? Devis { get; set; }

        [ForeignKey("RespServiceFinance")]

        public int? RespServiceFinanceId { get; set; }


    }
}
