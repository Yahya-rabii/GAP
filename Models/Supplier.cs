using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("Supplier", Title = "Supplier")]
    public class Supplier : User
    {
        [Required]
        [StringLength(255)]
        [SwaggerSchema("Company Name", Description = "The name of the company.")]
        public string? CompanyName { get; set; }

        [Required]
        [StringLength(255)]
        [SwaggerSchema("Address", Description = "The address of the company.")]
        public string? Adresse { get; set; }

        [Required]
        [SwaggerSchema("Postal Code", Description = "The postal code of the company's address.")]
        public int? PostalCode { get; set; }

        [Required]
        [SwaggerSchema("Phone Number", Description = "The phone number of the company.")]
        public int? PhoneNumber { get; set; }

        [DefaultValue(0)]
        [SwaggerSchema("Transaction Number", Description = "The transaction number for the supplier.")]
        public int TransactionNumber { get; set; } = 0;

        private List<SaleOffer> SaleOffers;

        [DefaultValue(false)]
        [SwaggerSchema("Is Valid", Description = "Indicates whether the supplier is valid.")]
        public bool IsValid { get; set; } = false;

        public Supplier() : base()
        {
            CompanyName = string.Empty;
            Adresse = string.Empty;
            PostalCode = 0;
            PhoneNumber = 0;
            TransactionNumber = 0;
            SaleOffers = new List<SaleOffer>();
            IsValid = false;
        }

        public Supplier(string email, string password, string fn, string ln, string Company, string address, int codepostal, int phonenum) : base(email, password, fn, ln)
        {
            CompanyName = Company;
            Adresse = address;
            PostalCode = codepostal;
            PhoneNumber = phonenum;
            TransactionNumber = 0;
            SaleOffers = new List<SaleOffer>();
            IsValid = false;
        }
    }
}
