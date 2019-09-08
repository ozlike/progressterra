using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Progressterra.Context
{
    public class ProgressterraContext: DbContext
    {
        public ProgressterraContext(DbContextOptions<ProgressterraContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
