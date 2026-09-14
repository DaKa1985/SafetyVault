using Microsoft.EntityFrameworkCore;
using SafetyVault.Api.Entities;

namespace SafetyVault.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Checklist> Checklists => Set<Checklist>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<ChecklistItem> ChecklistItems => Set<ChecklistItem>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ChecklistItem>()
                .HasOne(ci => ci.Item)
                .WithMany(i => i.ChecklistItems)
                .HasForeignKey(ci => ci.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
