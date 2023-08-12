namespace GAP.Models
{
    public class Stock
    {
        public int StockID { get; set; }
        public List<Product> Products { get; set; } 

        public Stock()
        {
            Products = new List<Product>();
        }
    }
}
