using FuturesPrice.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace FuturesPrice.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<FuturesPriceModel> FuturesPrices { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FuturesPriceModel>()
                .Property(f => f.PriceDifference)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<LogEntry>()
                .Property(l => l.Message)
                .IsRequired();

            modelBuilder.Entity<LogEntry>()
                .Property(l => l.Level)
                .IsRequired();

            modelBuilder.Entity<LogEntry>()
                .Property(l => l.Exception)
                .IsRequired(false); 
        }
    }
}
