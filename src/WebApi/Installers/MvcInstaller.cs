using WebApi.Installers.Interfaces;

namespace WebApi.Installers;

public sealed class MvcInstaller : IInstaller
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddMvc();
        services.AddEndpointsApiExplorer();
    }
}