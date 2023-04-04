namespace WebApi.Installers.Interfaces;

public interface IInstaller
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment);
}