using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("Finance Department Manager", Title = "Finance Department Manager")]
    public class FinanceDepartmentManager : User
    {
        private List<Bill> Billshistory;

        public FinanceDepartmentManager() : base()
        {
            Billshistory = new List<Bill>();
        }

        public FinanceDepartmentManager(string? email, string? password, string? fn, string? ln) : base(email, password, fn, ln)
        {
            Billshistory = new List<Bill>();
        }

        [SwaggerSchema("List of bills in the history", ReadOnly = true)]
        public List<Bill> HistoriqueBills
        {
            get { return Billshistory; }
            set { Billshistory = value; }
        }
    }
}
