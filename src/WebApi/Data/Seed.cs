using System.Diagnostics.CodeAnalysis;
using Domain.Constants.Security.Identity;
using Domain.Entities;
using Infrastructure.Data;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Data;

public sealed class Seed : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    
    public Seed(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Seed>>();

        await SeedSystemAdminRole(roleManager, logger);
        await SeedTenantAdminRole(roleManager, logger);
        await SeedDefaultUserRole(roleManager, logger);
        await SeedSystemAdminUser(userManager, logger);
        await SeedTenantAdminUser(userManager, logger);
        await SeedDefaultUser(userManager, logger);
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void Log<TEntity>(Result<TEntity> result, ILogger<Seed> logger) where TEntity : class
    {
        _ = result.Match<TEntity>(
            obj =>
            {
                switch (obj)
                {
                    case UserEntity user:
                        logger.LogInformation("[Service]: '{Service}' [Message]: 'User with ID '{UserID}' and username '{UserName}' has been successfully created'",
                            nameof(Seed),
                            user.Id.ToString(),
                            user.UserName);
                        break;
                    case RoleEntity role:
                        logger.LogInformation("[Service]: '{Service}' [Message]: 'Role with ID '{RoleID}' and name '{RoleName}' has been successfully created'",
                            nameof(Seed),
                            role.Id.ToString(),
                            role.Name);
                        break;
                }

                return obj;
            }, exception =>
            {
                logger.LogError("[Service]: '{Service}' [Message]: '{Message}'",
                    nameof(Seed),
                    exception.Message);
                return null!;
            });
    }

    private async Task SeedSystemAdminUser(UserManager<UserEntity> userManager, ILogger<Seed> logger)
    {
        const string password = "Pass123$systemAdmin";
        UserEntity systemAdmin = new()
        {
            Email = "system.admin@prov.dev",
            EmailConfirmed = true,
            LockoutEnabled = true,
            UserName = "system.admin@prov.dev"
        };

        var result = await systemAdmin.CreateAsync(password, userManager);
        if (result.IsSuccess)
        {
            result = await systemAdmin.AssignRolesAsync(new [] { Roles.SystemAdmin, Roles.TenantAdmin, Roles.DefaultUser }, userManager);
            Log(result, logger);
            return;
        }
        
        Log(result, logger);
    }
    
    private async Task SeedTenantAdminUser(UserManager<UserEntity> userManager, ILogger<Seed> logger)
    {
        const string password = "Pass123$tenantAdmin";
        UserEntity tenantAdmin = new()
        {
            Email = "tenant.admin@prov.dev",
            EmailConfirmed = true,
            LockoutEnabled = true,
            UserName = "tenant.admin@prov.dev"
        };

        var result = await tenantAdmin.CreateAsync(password, userManager);
        if (result.IsSuccess)
        {
            result = await tenantAdmin.AssignRolesAsync(new [] { Roles.TenantAdmin, Roles.DefaultUser }, userManager);
            Log(result, logger);
            return;
        }

        Log(result, logger);
    }
    
    private async Task SeedDefaultUser(UserManager<UserEntity> userManager, ILogger<Seed> logger)
    {
        const string password = "Pass123$defaultUser";
        UserEntity defaultUser = new()
        {
            Email = "default.user@prov.dev",
            EmailConfirmed = true,
            LockoutEnabled = true,
            UserName = "default.user@prov.dev"
        };

        var result = await defaultUser.CreateAsync(password, userManager);
        if (result.IsSuccess)
        {
            result = await defaultUser.AssignRolesAsync(new [] { Roles.DefaultUser }, userManager);
            Log(result, logger);
            return;
        }
        
        Log(result, logger);
    }

    private async Task SeedSystemAdminRole(RoleManager<RoleEntity> roleManager, ILogger<Seed> logger)
    {
        RoleEntity systemAdmin = new() { Name = Roles.SystemAdmin };
        var result = await systemAdmin.CreateAsync(roleManager);
        if (result.IsSuccess)
        {
            result = await systemAdmin.AddClaimsAsync(roleManager, new []
            {
                Permissions.View,
                Permissions.Create,
                Permissions.Edit,
                Permissions.Delete
            });
            
            Log(result, logger);
            return;
        }
        
        Log(result, logger);
    }

    private async Task SeedTenantAdminRole(RoleManager<RoleEntity> roleManager, ILogger<Seed> logger)
    {
        RoleEntity tenantAdmin = new() { Name = Roles.TenantAdmin };
        var result = await tenantAdmin.CreateAsync(roleManager);
        if (result.IsSuccess)
        {
            result = await tenantAdmin.AddClaimsAsync(roleManager, new []
            {
                Permissions.View,
                Permissions.Create,
                Permissions.Edit
            });
            
            Log(result, logger);
            return;
        }
        
        Log(result, logger);
    }

    private async Task SeedDefaultUserRole(RoleManager<RoleEntity> roleManager, ILogger<Seed> logger)
    {
        RoleEntity defaultUser = new() { Name = Roles.DefaultUser };
        var result = await defaultUser.CreateAsync(roleManager);
        if (result.IsSuccess)
        {
            result = await defaultUser.AddClaimsAsync(roleManager, new [] { Permissions.View });
            Log(result, logger);
            return;
        }
        
        Log(result, logger);
    }
}