using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GAP.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }

        [Required]
        [StringLength(255)]
        public string? Name { get; set; }

        [Required]
        [StringLength(255)]
        public string? Email { get; set; }

        [Required]
        [StringLength(255)]
        public string? Password { get; set; }

 
        [Required]
        [StringLength(255)]
        public string? Adresse { get; set; }[Required]
        public int? PostalCode { get; set; }[Required]
        public int? PhoneNumber { get; set; }


        [DefaultValue(0)]
        public int TransactionNumber { get; set; } = 0;

        private List<SaleOffer> SaleOffers;

        [DefaultValue(false)]
        public bool IsValid { get; set; } = false;


    }

}
