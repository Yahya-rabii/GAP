using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class ReclamationReply 
    {


        public int ReclamationReplyID { get; set; }

        [ForeignKey("Reclamation")]

        public int? ReclamationID { get; set; }
        public string Answer { get; set; }





        public ReclamationReply()
        {
            ReclamationReplyID = 0;
            ReclamationID = 0;
            Answer = string.Empty;
        }
        public ReclamationReply(int reclamationReplyID, int reclamationID,  string answer)
        {

            ReclamationReplyID = reclamationReplyID;
            ReclamationID = reclamationID;
            Answer = answer;
        }      
     
    }
}
