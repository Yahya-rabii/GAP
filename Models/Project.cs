using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("Project", Title = "Project")]
    public class Project
    {
        public int ProjectID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Budget { get; set; }

        [SwaggerSchema("Product", Title = "Product")]
        public List<Product> Products { get; set; }

        public Project()
        {
            Products = new List<Product>();
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
            Name = string.Empty;
            Description = string.Empty;
            Budget = 0;
        }
    }
}
