using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("PurchasingDepartmentManager", Title = "Purchasing Department Manager")]
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

        [SwaggerSchema("Purchase Requests", Title = "Purchase Requests")]
        public List<PurchaseRequest> DemandesAchats
        {
            get { return purchaseRequests; }
            set { purchaseRequests = value; }
        }

        [SwaggerSchema("Purchase Quotes", Title = "Purchase Quotes")]
        public List<PurchaseQuote> PurchaseQuotes
        {
            get { return purchaseQuotes; }
            set { purchaseQuotes = value; }
        }
    }
}
