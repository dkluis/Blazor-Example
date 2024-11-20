using Microsoft.EntityFrameworkCore;

namespace _4LL_Monitoring.Models;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbContext(DbContextOptions<DbContext> options) : base(options) { }

    // DbSets and other configurations
    public virtual DbSet<CollectApiDatum> Collectedapidata { get; set; }
    public virtual DbSet<AdminFunction>     AdminFunctions   { get; set; }
    public virtual DbSet<AdminRole>         AdminRoles       { get; set; }
    public virtual DbSet<AdminUser>         AdminUsers       { get; set; }
    public virtual DbSet<AdminApp>          AdminApps        { get; set; }
    public virtual DbSet<AdminUserAppState> AdminUserAppStates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CollectApiDatum>(
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

        // Ensure the unique constraint on AdminUser.UserID
        modelBuilder.Entity<AdminUser>()
                    .HasIndex(u => u.UserID)
                    .IsUnique();

        // Ensure the unique constraint on AdminApp.AppID
        modelBuilder.Entity<AdminApp>()
                    .HasIndex(a => a.AppID)
                    .IsUnique();

        // Configure the foreign key relationship between AdminApp and AdminFunction
        modelBuilder.Entity<AdminApp>()
                    .HasOne(a => a.Function)
                    .WithMany()
                    .HasForeignKey(a => a.FunctionID)
                    .OnDelete(DeleteBehavior.Cascade);

        // Ensure the composite unique constraint on AdminUserAppState (UserID, AppID)
        modelBuilder.Entity<AdminUserAppState>()
                    .HasKey(u => new { u.UserID, u.AppID });

        // Configure the foreign key relationships for AdminUserAppState
        modelBuilder.Entity<AdminUserAppState>()
                    .HasOne(u => u.User)
                    .WithMany(u => u.UserAppStates)
                    .HasForeignKey(u => u.UserID)
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AdminUserAppState>()
                    .HasOne(u => u.App)
                    .WithMany(a => a.UserAppStates)
                    .HasForeignKey(u => u.AppID)
                    .OnDelete(DeleteBehavior.Cascade);
    }
}
