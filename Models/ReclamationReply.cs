using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("Reclamation Reply", Title = "Reclamation Reply")]
    public class ReclamationReply
    {
        [SwaggerSchema("Reclamation Reply ID", Description = "The ID of the reclamation reply.")]
        public int ReclamationReplyID { get; set; }

        [ForeignKey("Reclamation")]
        [SwaggerSchema("Reclamation ID", Description = "The ID of the reclamation associated with the reply.")]
        public int? ReclamationID { get; set; }

        [SwaggerSchema("Answer", Description = "The answer provided in the reclamation reply.")]
        public string Answer { get; set; }

        public ReclamationReply()
        {
            ReclamationReplyID = 0;
            ReclamationID = 0;
            Answer = string.Empty;
        }

        public ReclamationReply(int reclamationReplyID, int reclamationID, string answer)
        {
            ReclamationReplyID = reclamationReplyID;
            ReclamationID = reclamationID;
            Answer = answer;
        }
    }
}
