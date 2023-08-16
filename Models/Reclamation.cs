using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class Reclamation 
    {


        public int ReclamationID { get; set; }
        public string ReclamationTitle { get; set; }

        [ForeignKey("User")]

        public int? UserID { get; set; }
        public string Description{ get; set; }

        public byte[]? BugPicture { get; set; }

        public DateTime CreationDate { get; set; }

        public Reclamation() 
        {
            UserID = 0;
            Description=string.Empty;
            BugPicture = new byte[0];
            CreationDate = DateTime.Now.Date;
        }
        public Reclamation(int notificationID,string notificationTitle, int userID , string desc , byte[] bugP) 
        {
            UserID = userID;
            Description = desc;
            BugPicture = bugP;
            CreationDate = DateTime.Now.Date;

        }

    }
}
