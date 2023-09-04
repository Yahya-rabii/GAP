using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("PurchaseRequest", Title = "Purchase Request")]
    public class PurchaseRequest
    {
        public int PurchaseRequestID { get; set; }

        [SwaggerSchema("Creation Date", Title = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Required]
        [StringLength(255)]
        [SwaggerSchema("Description", Title = "Description")]
        public string? Description { get; set; }

        [SwaggerSchema("Budget", Title = "Budget")]
        public double Budget { get; set; }

        [SwaggerSchema("IsValid", Title = "Is Valid")]
        public bool IsValid { get; set; } = true;

        public PurchaseRequest()
        {
            CreationDate = DateTime.Now.Date;
        }
    }
}
