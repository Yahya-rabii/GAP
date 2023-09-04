using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("BillPurchase", Title = "BillPurchase")]
    public class BillService:Bill
    {


       
        
        
        [ForeignKey("ServiceQuote")]
        [SwaggerSchema("ID of the associated Service Quote")]
        public int ServiceQuoteID { get; set; }




        public BillService():base()
        {
           
            ServiceQuoteID = 0;

        }

        public BillService(int billID, bool validity, int financeDepartmentManagerId, int serviceQuoteID) :base (billID, validity, financeDepartmentManagerId)
        {
            ServiceQuoteID = serviceQuoteID;
        }
    }
}
