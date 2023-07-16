using System.ComponentModel.DataAnnotations;

namespace GAP.Helper
{
    public class HistoryU
    {


        public int HistoryUID { get; set; }

        [Required]
        [StringLength(255)]
        public string? Email { get; set; }
        
        [Required]
        [StringLength(255)]
        public string? Titulair { get; set; }

    public HistoryU(int historyUID, string email , string? tit)
        {
            HistoryUID = historyUID;
            Email = email;
            Titulair = tit;

        }    public HistoryU()
        {
            HistoryUID = 0;
            Email = string.Empty;
            Titulair = string.Empty;

        }

    }

    
}
