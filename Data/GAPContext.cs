using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GAP.Models;

namespace GAP.Data
{
    public class GAPContext : DbContext
    {
        public GAPContext (DbContextOptions<GAPContext> options)
            : base(options)
        {
        }

        public void InitializeData()
        {
            var dbInitializer = new DbInitializer(this);
            dbInitializer.Seed();
        }


        public DbSet<GAP.Models.User> User { get; set; } = default!;

        public DbSet<GAP.Models.PurchaseQuote> PurchaseQuote { get; set; } = default!;

        public DbSet<GAP.Models.Bill> Bill { get; set; } = default!;

        public DbSet<GAP.Models.Supplier> Supplier { get; set; } = default!;

        public DbSet<GAP.Models.ReceptionReport> ReceptionReport { get; set; } = default!;

        public DbSet<GAP.Models.QualityTestReport> QualityTestReport { get; set; } = default!;

        public DbSet<GAP.Models.PurchasingReceptionist> PurchasingReceptionist { get; set; } = default!;

        public DbSet<GAP.Models.PurchasingDepartmentManager> PurchasingDepartmentManager { get; set; } = default!;

        public DbSet<GAP.Models.FinanceDepartmentManager> FinanceDepartmentManager { get; set; } = default!;

        public DbSet<GAP.Models.QualityTestingDepartmentManager> QualityTestingDepartmentManager { get; set; } = default!;

        public DbSet<GAP.Models.PurchaseRequest> PurchaseRequest { get; set; } = default!;

        public DbSet<GAP.Models.SaleOffer> SaleOffer { get; set; } = default!;

        public DbSet<GAP.Models.Product> Product { get; set; } = default!;


        public DbSet<GAP.Models.Notification> Notification { get; set; } = default!;

        public DbSet<GAP.Models.Sanction> Sanction { get; set; } = default!;

        public DbSet<GAP.Models.NotificationAdmin> NotificationAdmin { get; set; } = default!;

        public DbSet<GAP.Models.NotificationInfo> NotificationInfo { get; set; } = default!;

        public DbSet<GAP.Models.NotificationSupplier> NotificationSupplier { get; set; } = default!;

        public DbSet<GAP.Models.Project> Project { get; set; } = default!;

        public DbSet<GAP.Models.ProjectManager> ProjectManager { get; set; } = default!;

        public DbSet<GAP.Models.Stock> Stock { get; set; } = default!;


    }
}
