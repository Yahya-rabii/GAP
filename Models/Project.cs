namespace GAP.Models
{
    public class Project
    {

        public int ProjectID { get; set; }
        public DateTime StartDate{ get; set; }
        public DateTime EndDate{ get; set; }
        public String Name{ get; set; }
        public String Description { get; set; }
        public double Budget { get; set; }

        public List<Product> Products { get; set; }



        public Project() 
        {

            Products = new List<Product>();
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
            Name=string.Empty;
            Description = string.Empty;
            Budget = 0;

            
        }

    }
}
