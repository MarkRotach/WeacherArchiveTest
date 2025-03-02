using Microsoft.EntityFrameworkCore;
using WeatherArchive.Database.Domain;

namespace WeatherArchive.Database;

public sealed class WeatherArchiveDbContext(DbContextOptions<WeatherArchiveDbContext> options) 
    : DbContext(options)
{
    public DbSet<WeatherReport> WeatherReports { get; set; }
    public DbSet<WindDirection> WindDirections { get; set; }
    public DbSet<ReportWindDirection> ReportWindDirections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<WeatherReport>()
            .HasIndex(r => new { r.Date, r.Time });
        
        modelBuilder
            .Entity<WindDirection>()
            .HasData(
                new WindDirection { Id = 1, Name = "С" },
                new WindDirection { Id = 2, Name = "Ю" },
                new WindDirection { Id = 3, Name = "З" },
                new WindDirection { Id = 4, Name = "В" },
                new WindDirection { Id = 5, Name = "СЗ" },
                new WindDirection { Id = 6, Name = "СВ" },
                new WindDirection { Id = 7, Name = "ЮЗ" },
                new WindDirection { Id = 8, Name = "ЮВ" }
            );
        
        modelBuilder
            .Entity<ReportWindDirection>()
            .HasOne(rwd => rwd.Report)
            .WithMany(wr => wr.WindDirections);

        modelBuilder
            .Entity<ReportWindDirection>()
            .HasOne(rwd => rwd.WindDirection)
            .WithMany();
        
        base.OnModelCreating(modelBuilder);
    }
}