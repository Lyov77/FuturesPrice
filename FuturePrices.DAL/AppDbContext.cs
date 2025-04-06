using FuturesPrice.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace FuturesPrice.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<FuturesPriceModel> FuturesPrices { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FuturesPriceModel>()
                .Property(f => f.PriceDifference)
                .HasColumnType("decimal(18,2)");
        }
    }

    
}
