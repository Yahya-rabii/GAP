using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class OrdreCreationRTQ

    {
        public int OrdreCreationRTQID { get; set; }

        [ForeignKey("DevisID")]
        public int? DevisID { get; set; }
        public Devis? Devis { get; set; }

        [ForeignKey("RespServiceQualite")]

        public int? RespServiceQualiteId { get; set; }
    }
}
