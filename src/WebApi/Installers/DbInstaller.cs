using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Installers.Interfaces;

namespace WebApi.Installers;

public sealed class DbInstaller : IInstaller
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException(string.Format(
                                   "[Class]: '{0}' [Message]: 'Failed to retrieve connection string from the configuration file'",
                                   nameof(Program)));
        
        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(connectionString, buider => buider.MigrationsAssembly(nameof(WebApi)));
            options.EnableSensitiveDataLogging(environment.IsDevelopment());
            options.EnableServiceProviderCaching();
            options.UseLoggerFactory(DataContext.PropertyLoggerFactory);
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();

        services.AddScoped<IIdentityManager, IdentityManager>();
        services.AddHostedService<Seed>();
    }
}