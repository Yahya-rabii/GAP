using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("Reclamations History", Title = "Reclamations History")]
    public class ReclamationsHistory
    {
        [SwaggerSchema("Reclamations History ID", Description = "The ID of the reclamations history.")]
        public int ReclamationsHistoryID { get; set; }

        [ForeignKey("Reclamation")]
        [SwaggerSchema("Reclamation ID", Description = "The ID of the reclamation associated with the reclamations history.")]
        public int ReclamationsID { get; set; }
    }
}
