using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class RapportTestQualite
    {
        public int Id { get; set; }
        public bool ValiditeEtat { get; set; }
        public bool ValiditeNbrPiece { get; set; }
        public bool ValiditeFonctionnement { get; set; }

        [ForeignKey("RespServiceQualite")]
        public int RespServiceQualiteId { get; set; } // Foreign key property

        public RespServiceQualite RespServiceQualite { get; set; } // Navigation property
    }
}
