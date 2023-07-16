using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GAP.Models
{
    public class RespServiceFinance 
    {
        public int RespServiceFinanceID { get; set; }

        [Required]
        [StringLength(255)]
        public string? Email { get; set; }

        [Required]
        [StringLength(255)]
        public string? Password { get; set; }

        [Required]
        [StringLength(255)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string? LastName { get; set; }

        private List<Facture> historiqueFactures;

        public RespServiceFinance()
        {
            RespServiceFinanceID = 0;
            Email = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            historiqueFactures = new List<Facture>();
        }

        public RespServiceFinance(int receptServiceAchatID, string? email, string? password, string? fn, string? ln)
        {
            RespServiceFinanceID = receptServiceAchatID;
            Email = email;
            Password = password;
            FirstName = fn;
            LastName = ln;
            historiqueFactures = new List<Facture>();
        }



        public List<Facture> HistoriqueFactures
        {
            get { return historiqueFactures; }
            set { historiqueFactures = value; }
        }

   
    }

}
