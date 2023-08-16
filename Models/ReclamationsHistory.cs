using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class ReclamationsHistory
    {


        public int ReclamationsHistoryID { get; set; }
        
        
        [ForeignKey("Reclamation")]
        public int ReclamationsID { get; set; }

    
        

    }
}
