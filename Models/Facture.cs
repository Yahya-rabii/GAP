using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class Facture
    {
        public int FactureID { get; set; }
        public bool Validite { get; set; }
        public int RespServiceFinanceId { get; set; }
        [ForeignKey("DevisID")]
        public int? DevisID { get; set; }
    }
}