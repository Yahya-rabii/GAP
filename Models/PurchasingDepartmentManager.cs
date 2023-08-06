using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GAP.Models
{
    public class PurchasingDepartmentManager : User
    {


        private List<PurchaseRequest> purchaseRequests;
        private List<PurchaseQuote> purchaseQuotes;



        public PurchasingDepartmentManager() : base()
        {
           
            purchaseRequests = new List<PurchaseRequest>();
            purchaseQuotes = new List<PurchaseQuote>(); 
        }

        public PurchasingDepartmentManager(string? email, string? password, string? fn, string? ln) : base(email, password, fn, ln)
        {
          
            purchaseRequests = new List<PurchaseRequest>();
            purchaseQuotes = new List<PurchaseQuote>();
        }


        public List<PurchaseRequest> DemandesAchats
        {
            get { return purchaseRequests; }
            set { purchaseRequests = value; }
        }

        public List<PurchaseQuote> PurchaseQuotes
        {
            get { return purchaseQuotes; }
            set { purchaseQuotes = value; }
        }
    }
}
