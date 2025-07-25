using Microsoft.EntityFrameworkCore;
using SimpleInventoryTracker.Models;

namespace SimpleInventoryTracker.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .Property(i => i.ItemId)
                .ValueGeneratedNever(); 
        }
    }
}
