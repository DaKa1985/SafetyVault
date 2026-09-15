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
            //Checklist- und Item-Name auf 100 Zeichen begrenzen
            modelBuilder.Entity<Checklist>()
                .Property(c => c.Name)
                .HasMaxLength(100);
            modelBuilder.Entity<Item>()
                .Property(i => i.Name)
                .HasMaxLength(100);
            //Description auf 500 Zeichen begrenzen
            modelBuilder.Entity<Checklist>()
                .Property(c => c.Description)
                .HasMaxLength(500);
            modelBuilder.Entity<Item>()
                .Property(i => i.Description)
                .HasMaxLength(500);
        }
    }
}
