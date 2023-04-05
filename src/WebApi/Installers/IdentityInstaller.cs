using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using OpenIddict.Abstractions;
using WebApi.Installers.Interfaces;

namespace WebApi.Installers;

public sealed class IdentityInstaller : IInstaller
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services
            .AddIdentity<UserEntity, RoleEntity>()
            .AddEntityFrameworkStores<DataContext>();

        services.Configure<IdentityOptions>(option =>
        {
            // Claims
            option.ClaimsIdentity.UserNameClaimType = "name";
            option.ClaimsIdentity.UserIdClaimType = "sub";
            option.ClaimsIdentity.RoleClaimType = "role";
            option.ClaimsIdentity.EmailClaimType = "email";
            option.SignIn.RequireConfirmedAccount = false;

            // Password settings
            option.Password.RequireDigit = true;
            option.Password.RequireLowercase = true;
            option.Password.RequireUppercase = true;
            option.Password.RequireNonAlphanumeric = true;
            option.Password.RequiredLength = 6;
            option.Password.RequiredUniqueChars = 1;

            // Lockout settings
            option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            option.Lockout.MaxFailedAccessAttempts = 5;
            option.Lockout.AllowedForNewUsers = true;

            // User settings
            option.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            option.User.RequireUniqueEmail = true;
        });
        
        services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(1));
    }
}