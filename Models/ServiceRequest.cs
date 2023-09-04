using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("ServiceRequest", Title = "ServiceRequest")]
    public class ServiceRequest
    {
        [SwaggerSchema("ServiceRequest ID")]
        public int ServiceRequestID { get; set; }

        [SwaggerSchema("Creation Date", Title = "Creation Date")]
        public DateTime CreationDate { get; set; }


        [Required]
        [StringLength(255)]
        [SwaggerSchema("Title", Title = "Title")]
        public string? Title { get; set; }


        [Required]
        [StringLength(255)]
        [SwaggerSchema("Description", Title = "Description")]
        public string? Description { get; set; }   
        

  

        [SwaggerSchema("IsValid", Title = "Is Valid")]
        public bool IsValid { get; set; } = true;



        public byte[]? ServiceRequestPicture { get; set; }



        public ServiceRequest()
        {
            CreationDate = DateTime.Now.Date;
        }
    }
}
