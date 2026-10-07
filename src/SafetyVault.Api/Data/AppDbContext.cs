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
        public DbSet<StockBatch> StockBatches => Set<StockBatch>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ChecklistItem>()
                .HasOne(ci => ci.Item)
                .WithMany(i => i.ChecklistItems)
                .HasForeignKey(ci => ci.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
            //Artikel mit Chargen darf nicht gelöscht werden, wenn Chargen existieren, Bestand würde sonst verloren gehen
            modelBuilder.Entity<StockBatch>()
                .HasOne(sb => sb.Item)
                .WithMany(i => i.StockBatches)
                .HasForeignKey(sb => sb.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
            //Checklist- und Item-Name auf 100 Zeichen begrenzen
            modelBuilder.Entity<Checklist>()
                .Property(c => c.Name)
                .HasMaxLength(100);
            modelBuilder.Entity<Item>()
                .Property(i => i.Name)
                .HasMaxLength(100);
            //Artikelname eindeutig?
            modelBuilder.Entity<Item>()
                .HasIndex(i => i.Name)
                .IsUnique();
            //Index auf ItemId und BestBefore in StockBatch, um doppelte Chargen für denselben Artikel und dasselbe Haltbarkeitsdatum zu verhindern, auch ohne MHD nur eine Charge pro Artikel -> kein Filter
            modelBuilder.Entity<StockBatch>()
                .HasIndex(sb => new { sb.ItemId, sb.BestBefore })
                .IsUnique()
                .HasFilter(null);
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
