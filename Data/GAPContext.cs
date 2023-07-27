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

        public DbSet<GAP.Models.Devis> Devis { get; set; } = default!;

        public DbSet<GAP.Models.Facture> Facture { get; set; } = default!;

        public DbSet<GAP.Models.Fournisseur> Fournisseur { get; set; } = default!;

        public DbSet<GAP.Models.RapportReception> RapportReception { get; set; } = default!;

        public DbSet<GAP.Models.RapportTestQualite> RapportTestQualite { get; set; } = default!;

        public DbSet<GAP.Models.ReceptServiceAchat> ReceptServiceAchat { get; set; } = default!;

        public DbSet<GAP.Models.RespServiceAchat> RespServiceAchat { get; set; } = default!;

        public DbSet<GAP.Models.RespServiceFinance> RespServiceFinance { get; set; } = default!;

        public DbSet<GAP.Models.RespServiceQualite> RespServiceQualite { get; set; } = default!;

        public DbSet<GAP.Models.DemandeAchat> DemandeAchat { get; set; } = default!;

        public DbSet<GAP.Models.OffreVente> OffreVente { get; set; } = default!;

        public DbSet<GAP.Models.Produit> Produit { get; set; } = default!;

        public DbSet<GAP.Models.OrdreCreationFacture> OrdreCreationFacture { get; set; } = default!;

        public DbSet<GAP.Models.OrdreCreationRTQ> OrdreCreationRTQ { get; set; } = default!;

        public DbSet<GAP.Models.Notification> Notification { get; set; } = default!;


    }
}
