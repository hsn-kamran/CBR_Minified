using Microsoft.Extensions.Configuration;

namespace CBR_Minified;

public static class ConnectionHelper
{
    public static string GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json", true, true)
            .Build();

        string cs = configuration.GetSection("ConnectionStrings:CBR_Postgres_Connection").Value
            ?? throw new ArgumentNullException(nameof(configuration));

        return cs;
    }
}
