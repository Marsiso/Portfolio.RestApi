using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Options;

public static class DbContextOptions
{
    public static void ConfigureDbContext(this DbContextOptionsBuilder builder,
        string connectionString, bool isDevelopment)
    {
        builder.UseSqlServer(connectionString);
        builder.EnableSensitiveDataLogging(isDevelopment);
        builder.EnableServiceProviderCaching();
        builder.UseLoggerFactory(DataContext.PropertyLoggerFactory);
    }
}