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

        public DbSet<GAP.Models.User> User { get; set; } = default!;
    }
}
