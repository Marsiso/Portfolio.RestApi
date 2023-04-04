using Microsoft.OpenApi.Models;
using WebApi.Installers.Interfaces;

namespace WebApi.Installers;

public sealed class SwaggerInstaller : IInstaller
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "RESTful Web API",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Email = "olsak.marek@outlook.cz",
                    Name = "Marek Olšák",
                    Url = new Uri("https://www.linkedin.com/in/marek-ol%C5%A1%C3%A1k-1715b724a/")
                },
                Description = "RESTful Web API to demonstrate .NET knowledge"
            });
        });
    }
}