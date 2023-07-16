using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GAP.Models
{
   

        public class ReceptServiceAchat : User
    {

       

        private List<RapportReception> historiqueRapportsReceptions;

        public ReceptServiceAchat() :base() 
        {
            
            historiqueRapportsReceptions = new List<RapportReception>();
        } 
        
        public ReceptServiceAchat(string? email , string? password , string? fn , string? ln) : base(email, password, fn, ln)
        {
           
            historiqueRapportsReceptions = new List<RapportReception>();
        }
        
        public List<RapportReception> HistoriqueRapportsReceptions
            {
                get { return historiqueRapportsReceptions; }
                set { historiqueRapportsReceptions = value; }
            }
        }


    

}
