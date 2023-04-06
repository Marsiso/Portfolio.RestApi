using System.Diagnostics.CodeAnalysis;
using Application.Interfaces;
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
        var identityManager = scope.ServiceProvider.GetRequiredService<IIdentityManager>();
        
        await SeedSystemAdminRoleAsync(identityManager);
        await SeedTenantAdminRoleAsync(identityManager);
        await SeedDefaultUserRoleAsync(identityManager);
        await SeedSystemAdminUserAsync(identityManager);
        await SeedTenantAdminUserAsync(identityManager);
        await SeedDefaultUserAsync(identityManager);
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task SeedSystemAdminRoleAsync(IIdentityManager identityManger)
    {
        var permissions = new[] { Permissions.View, Permissions.Create, Permissions.Edit, Permissions.Delete };
        RoleEntity systemAdminRole = new() { Name = Roles.SystemAdmin };
        
        await identityManger.TryCreateRoleAsync(systemAdminRole);
        await identityManger.TryAddRoleClaimsAsync(systemAdminRole, "Permission", permissions);
    }
    
    private async Task SeedTenantAdminRoleAsync(IIdentityManager identityManger)
    {
        var permissions = new[] { Permissions.View, Permissions.Create, Permissions.Edit};
        RoleEntity tenantAdminRole = new() { Name = Roles.TenantAdmin };
        
        await identityManger.TryCreateRoleAsync(tenantAdminRole);
        await identityManger.TryAddRoleClaimsAsync(tenantAdminRole, "Permission", permissions);
    }
    
    private async Task SeedDefaultUserRoleAsync(IIdentityManager identityManger)
    {
        var permissions = new[] { Permissions.View };
        RoleEntity defaultUserRole = new() { Name = Roles.DefaultUser };

        await identityManger.TryCreateRoleAsync(defaultUserRole);
        await identityManger.TryAddRoleClaimsAsync(defaultUserRole, "Permission", permissions);
    }

    private async Task SeedSystemAdminUserAsync(IIdentityManager identityManger)
    {
        const string password = "Pass123$systemAdmin";
        var roles = new[] { Roles.SystemAdmin, Roles.TenantAdmin, Roles.DefaultUser };
        UserEntity systemAdmin = new()
        {
            Email = "system.admin@prov.dev",
            EmailConfirmed = true,
            LockoutEnabled = true,
            UserName = "system.admin@prov.dev"
        };

        await identityManger.TryCreateUserAsync(systemAdmin, password);
        await identityManger.TryAssignRolesAsync(systemAdmin, roles);
    }
    
    private async Task SeedTenantAdminUserAsync(IIdentityManager identityManger)
    {
        const string password = "Pass123$tenantAdmin";
        var roles = new[] { Roles.TenantAdmin, Roles.DefaultUser };
        UserEntity tenantAdmin = new()
        {
            Email = "tenant.admin@prov.dev",
            EmailConfirmed = true,
            LockoutEnabled = true,
            UserName = "tenant.admin@prov.dev"
        };
        
        await identityManger.TryCreateUserAsync(tenantAdmin, password);
        await identityManger.TryAssignRolesAsync(tenantAdmin, roles);
    }
    
    private async Task SeedDefaultUserAsync(IIdentityManager identityManger)
    {
        const string password = "Pass123$defaultUser";
        var roles = new[] { Roles.DefaultUser };
        UserEntity defaultUser = new()
        {
            Email = "default.user@prov.dev",
            EmailConfirmed = true,
            LockoutEnabled = true,
            UserName = "default.user@prov.dev"
        };
        
        await identityManger.TryCreateUserAsync(defaultUser, password);
        await identityManger.TryAssignRolesAsync(defaultUser, roles);
    }
}