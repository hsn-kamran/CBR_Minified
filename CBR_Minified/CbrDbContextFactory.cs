using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CBR_Minified;

/// чтобы работали миграции 
/// <see cref="https://learn.microsoft.com/en-gb/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli" />
public class CbrDbContextFactory : IDesignTimeDbContextFactory<CbrDbContext>
{
    public CbrDbContext CreateDbContext(string[] args)
    {        
        var options = new DbContextOptionsBuilder<CbrDbContext>()
            .UseNpgsql(ConnectionHelper.GetConnectionString()).Options;

        return new CbrDbContext(options);
    }
}
