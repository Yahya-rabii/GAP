using System.ComponentModel.DataAnnotations;

namespace GAP.Models
{
    public class RespServiceQualite 
    {

        public int RespServiceQualiteID { get; set; }

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

        private List<RapportTestQualite> historiqueRapportQualite;

        public RespServiceQualite()
        {
            RespServiceQualiteID = 0;
            Email = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            historiqueRapportQualite= new List<RapportTestQualite>();
        }

        public RespServiceQualite(int receptServiceAchatID, string? email, string? password, string? fn, string? ln)
        {
            RespServiceQualiteID = receptServiceAchatID;
            Email = email;
            Password = password;
            FirstName = fn;
            LastName = ln;
            historiqueRapportQualite = new List<RapportTestQualite>();
        }



        public List<RapportTestQualite> HistoriqueRapportQualite
        {
            get { return historiqueRapportQualite; }
            set { historiqueRapportQualite = value; }
        }

    }

}
