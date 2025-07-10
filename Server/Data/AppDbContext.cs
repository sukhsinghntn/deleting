using DynamicFormsApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicFormsApp.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Form> Forms { get; set; }
        public DbSet<FormField> FormFields { get; set; }
        public DbSet<DynamicFormsApp.Shared.Models.FormShare> FormShares { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormField>()
                .Property(f => f.OptionsJson)
                .HasColumnType("nvarchar(max)");
        }
    }
}
