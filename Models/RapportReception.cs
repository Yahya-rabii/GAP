using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
   public class RapportReception
{
    private int id;
    private DateTime date;

        [ForeignKey("ReceptServiceAchat")]
        public int ReceptServiceAchatId { get; set; } // Foreign key property
        public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public DateTime Date
    {
        get { return date; }
        set { date = value; }
    }

        public RapportReception()
        {
            date = DateTime.Now.Date;

        }
    }

}
