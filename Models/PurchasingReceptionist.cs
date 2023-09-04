using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("PurchasingReceptionist", Title = "Purchasing Receptionist")]
    public class PurchasingReceptionist : User
    {
        private List<ReceptionReport> ReceptionReportHistory;

        public PurchasingReceptionist() : base()
        {
            ReceptionReportHistory = new List<ReceptionReport>();
        }

        public PurchasingReceptionist(string? email, string? password, string? fn, string? ln) : base(email, password, fn, ln)
        {
            ReceptionReportHistory = new List<ReceptionReport>();
        }

        [SwaggerSchema("Reception Report History", Title = "Reception Report History")]
        public List<ReceptionReport> HistoriqueRapportsReceptions
        {
            get { return ReceptionReportHistory; }
            set { ReceptionReportHistory = value; }
        }
    }
}
