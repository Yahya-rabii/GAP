using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GAP.Models
{
    public class Supplier:User
    {

        [Required]
        [StringLength(255)]
        public string? CompanyName { get; set; }


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





        public Supplier() : base()
        {
            CompanyName = string.Empty;
            Adresse = string.Empty;
            PostalCode = 0;
            PhoneNumber = 0;
            TransactionNumber= 0;
            SaleOffers = new List<SaleOffer>();
            IsValid = false;
        }

        public Supplier(string email, string password, string fn, string ln ,string Company,string address ,int codepostal,int phonenum) : base(email, password, fn, ln)
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
