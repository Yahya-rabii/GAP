using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("Bill", Title = "Bill")]
    public class Bill
    {
        [SwaggerSchema("Bill ID")]
        public int BillID { get; set; }

        [SwaggerSchema("Validity status")]
        public bool Validity { get; set; }

        [SwaggerSchema("ID of the Finance Department Manager")]
        public int FinanceDepartmentManagerId { get; set; }




        public Bill() 
        {
           BillID = 0;
            Validity = false;
            FinanceDepartmentManagerId = 0;
        
        }

        public Bill(int billID, bool validity, int financeDepartmentManagerId)
        {
            BillID = billID;
            Validity = validity;
            FinanceDepartmentManagerId = financeDepartmentManagerId;
  
        }
    }
}
