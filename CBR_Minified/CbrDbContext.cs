using CBR_Minified.DbModels;
using Microsoft.EntityFrameworkCore;

namespace CBR_Minified;

public class CbrDbContext : DbContext
{
    public CbrDbContext(DbContextOptions<CbrDbContext> options) : base(options)
    {
    }

    public DbSet<CurrencyCourse> CurrencyCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrencyCourse>()
            .HasKey(c => new { c.CurrencyId, c.Date });

        modelBuilder.Entity<CurrencyCourse>()
            .HasIndex(c => new { c.CurrencyId, c.Date });

        base.OnModelCreating(modelBuilder);
    }
}
