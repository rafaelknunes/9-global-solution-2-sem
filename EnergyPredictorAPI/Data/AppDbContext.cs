using Microsoft.EntityFrameworkCore;
using EnergyPredictorAPI.Models;

namespace EnergyPredictorAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ConsumptionData> ConsumptionData { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ConsumptionData>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DeviceId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Timestamp)
                      .HasColumnType("TIMESTAMP")
                      .IsRequired();
                entity.Property(e => e.Consumption)
                      .HasPrecision(18, 2)
                      .IsRequired();
                entity.Property(e => e.DeviceType)
                      .IsRequired()
                      .HasMaxLength(50);
            });
        }



    }
}
