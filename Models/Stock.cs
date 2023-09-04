using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("Stock", Title = "Stock")]
    public class Stock
    {
        [SwaggerSchema("Stock ID", Description = "The ID of the stock.")]
        public int StockID { get; set; }

        [SwaggerSchema("Products", Description = "The list of products in the stock.")]
        public List<Product> Products { get; set; }

        public Stock()
        {
            Products = new List<Product>();
        }
    }
}
