using System;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("Reclamation", Title = "Reclamation")]
    public class Reclamation
    {
        [SwaggerSchema("Reclamation ID", Description = "The ID of the reclamation.")]
        public int ReclamationID { get; set; }

        [SwaggerSchema("Reclamation Title", Description = "The title of the reclamation.")]
        public string ReclamationTitle { get; set; }

        [ForeignKey("User")]
        [SwaggerSchema("User ID", Description = "The ID of the user associated with the reclamation.")]
        public int? UserID { get; set; }

        [SwaggerSchema("Description", Description = "The description of the reclamation.")]
        public string Description { get; set; }

        [SwaggerSchema("Bug Picture", Description = "The picture related to the reclamation.")]
        public byte[]? BugPicture { get; set; }

        [SwaggerSchema("Creation Date", Description = "The date when the reclamation was created.")]
        public DateTime CreationDate { get; set; }

        public Reclamation()
        {
            UserID = 0;
            Description = string.Empty;
            BugPicture = new byte[0];
            CreationDate = DateTime.Now.Date;
        }

        public Reclamation(int notificationID, string notificationTitle, int userID, string desc, byte[] bugP)
        {
            UserID = userID;
            Description = desc;
            BugPicture = bugP;
            CreationDate = DateTime.Now.Date;
        }
    }
}
