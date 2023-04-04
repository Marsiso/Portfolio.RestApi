using Domain.Entities;
using Infrastructure.Data;
using WebApi.Installers.Interfaces;

namespace WebApi.Installers;

public sealed class IdentityInstaller : IInstaller
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services
            .AddIdentity<UserEntity, RoleEntity>()
            .AddEntityFrameworkStores<DataContext>();
    }
}