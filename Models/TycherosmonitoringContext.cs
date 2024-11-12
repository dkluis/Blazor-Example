using Microsoft.EntityFrameworkCore;

namespace _4LL_Monitoring.Models;

public class TycherosmonitoringContext : DbContext
{
    /*private readonly IConfiguration _configuration;

    public TycherosmonitoringContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("TycherosMonitoring");
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(11, 5, 2)));
        }
    }
    
    // Rest of the code remains unchanged
    public TycherosmonitoringContext(DbContextOptions<TycherosmonitoringContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Collectedapidatum> Collectedapidata { get; set; }*/

        public TycherosmonitoringContext(DbContextOptions<TycherosmonitoringContext> options)
                : base(options)
        {
        }

        // DbSets and other configurations
        public virtual DbSet<Collectedapidatum> Collectedapidata { get; set; }
}

    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_uca1400_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Collectedapidatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("collectedapidata");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.ApiName).HasMaxLength(255);
            entity.Property(e => e.Created)
                .HasMaxLength(6)
                .HasDefaultValueSql("utc_timestamp(6)");
            entity.Property(e => e.Date).HasComputedColumnSql("cast(`Created` as date)", true);
            entity.Property(e => e.Day)
                .HasComputedColumnSql("dayofmonth(`Created`)", true)
                .HasColumnType("int(11)");
            entity.Property(e => e.ErrorDetails).HasColumnType("text");
            entity.Property(e => e.Hour)
                .HasComputedColumnSql("hour(`Created`)", true)
                .HasColumnType("int(11)");
            entity.Property(e => e.HttpStatusCode).HasColumnType("int(11)");
            entity.Property(e => e.JsonResponse).HasColumnType("text");
            entity.Property(e => e.Minute)
                .HasComputedColumnSql("minute(`Created`)", true)
                .HasColumnType("int(11)");
            entity.Property(e => e.Month)
                .HasComputedColumnSql("month(`Created`)", true)
                .HasColumnType("int(11)");
            entity.Property(e => e.Note).HasColumnType("text");
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.Threshold).HasColumnType("int(11)");
            entity.Property(e => e.Type).HasMaxLength(20);
            entity.Property(e => e.Value).HasColumnType("int(11)");
            entity.Property(e => e.Year)
                .HasComputedColumnSql("year(`Created`)", true)
                .HasColumnType("int(11)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);*/
