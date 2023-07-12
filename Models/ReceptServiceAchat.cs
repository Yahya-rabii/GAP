using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GAP.Models
{
   

        public class ReceptServiceAchat : User
        {
            private List<RapportReception> historiqueRapportsReceptions;


            public List<RapportReception> HistoriqueRapportsReceptions
            {
                get { return historiqueRapportsReceptions; }
                set { historiqueRapportsReceptions = value; }
            }
        }


    

}
