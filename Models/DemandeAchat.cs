using GAP.Helper;

namespace GAP.Models
{
    public class DemandeAchat
    {
        private int id;
        private int date;
        private Desc description;
        private double budget;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Date
        {
            get { return date; }
            set { date = value; }
        }

        public Desc Description
        {
            get { return description; }
            set { description = value; }
        }

        public double Budget
        {
            get { return budget; }
            set { budget = value; }
        }
    }

}
