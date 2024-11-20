using Microsoft.EntityFrameworkCore;

namespace _4LL_Monitoring.Models;

public class TycherosmonitoringContext : DbContext
{
    public TycherosmonitoringContext(DbContextOptions<TycherosmonitoringContext> options) : base(options) { }

    // DbSets and other configurations
    public virtual DbSet<Collectedapidatum> Collectedapidata { get; set; }
    public virtual DbSet<AdminFunction>     AdminFunctions   { get; set; }
    public virtual DbSet<AdminRole>         AdminRoles       { get; set; }
    public virtual DbSet<AdminUser>         AdminUsers       { get; set; }
    public virtual DbSet<AdminApp>          AdminApps        { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Collectedapidatum>(
            entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Date).HasComputedColumnSql("CONVERT([date],[Created]) PERSISTED");

                entity.Property(e => e.Year).HasComputedColumnSql("datepart(year,[Created]) PERSISTED");

                entity.Property(e => e.Month).HasComputedColumnSql("datepart(month,[Created]) PERSISTED");

                entity.Property(e => e.Day).HasComputedColumnSql("datepart(day,[Created]) PERSISTED");

                entity.Property(e => e.Hour).HasComputedColumnSql("datepart(hour,[Created]) PERSISTED");

                entity.Property(e => e.Minute).HasComputedColumnSql("datepart(minute,[Created]) PERSISTED");
            }
        );

        modelBuilder.Entity<AdminFunction>().HasKey(f => f.FunctionID);
        modelBuilder.Entity<AdminFunction>().HasIndex(f => f.FunctionID).IsUnique();
        modelBuilder.Entity<AdminRole>().HasIndex(r => r.RoleID).IsUnique();
        modelBuilder.Entity<AdminUser>().HasIndex(u => u.UserID).IsUnique();
        modelBuilder.Entity<AdminApp>()
                    .HasOne(a => a.Function)
                    .WithMany()
                    .HasForeignKey(a => a.FunctionID)
                    .OnDelete(DeleteBehavior.Cascade);
    }
}
