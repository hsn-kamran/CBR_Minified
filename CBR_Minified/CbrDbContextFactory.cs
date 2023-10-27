using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CBR_Minified;

/// чтобы работали миграции 
/// <see cref="https://learn.microsoft.com/en-gb/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli" />
public class CbrDbContextFactory : IDesignTimeDbContextFactory<CbrDbContext>
{
    public CbrDbContext CreateDbContext(string[] args)
    {        
        var options = new DbContextOptionsBuilder<CbrDbContext>()
            .UseNpgsql(GetConnectionString()).Options;

        return new CbrDbContext(options);
    }

    private string GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json", true, true)
            .Build();

        string cs = configuration.GetSection("ConnectionStrings:CBR_Postgres_Connection").Value
            ?? throw new ArgumentNullException(nameof(configuration));

        return cs;
    }
}
