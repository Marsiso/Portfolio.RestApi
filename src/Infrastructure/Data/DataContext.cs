using System.Runtime.CompilerServices;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public sealed class DataContext : IdentityDbContext<
    UserEntity,
    RoleEntity,
    long,
    UserClaimEntity,
    UserRoleEntity,
    UserLoginEntity,
    RoleClaimEntity,
    UserTokenEntity>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        ReplaceDefaultIdentityMappingScheme(builder);
    }

    /// <summary>
    /// Renames the default ASP.NET Core Identity mapping scheme.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static ModelBuilder ReplaceDefaultIdentityMappingScheme(ModelBuilder builder)
    {
        const string dbSchemeName = "dbo";

        builder.Entity<UserEntity>(b =>
        {
            b.ToTable(nameof(UserEntity).Replace("Entity", string.Empty), dbSchemeName);
        });
        
        builder.Entity<RoleEntity>(b =>
        {
            b.ToTable(nameof(RoleEntity).Replace("Entity", string.Empty), dbSchemeName);
        });
        
        builder.Entity<UserClaimEntity>(b =>
        {
            b.ToTable(nameof(UserClaimEntity).Replace("Entity", string.Empty), dbSchemeName);
        });
        
        builder.Entity<RoleClaimEntity>(b =>
        {
            b.ToTable(nameof(RoleClaimEntity).Replace("Entity", string.Empty), dbSchemeName);
        });

        builder.Entity<UserRoleEntity>(b =>
        {
            b.HasKey(key => new { key.UserId, key.RoleId });
            b.ToTable(nameof(UserRoleEntity).Replace("Entity", string.Empty), dbSchemeName);
        });

        builder.Entity<UserLoginEntity>(b =>
        {
            b.HasKey(key => new { key.ProviderKey, key.LoginProvider });
            b.ToTable(nameof(UserLoginEntity).Replace("Entity", string.Empty), dbSchemeName);
        });

        builder.Entity<UserTokenEntity>(b =>
        {
            b.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
            b.ToTable(nameof(UserTokenEntity).Replace("Entity", string.Empty), dbSchemeName);
        });

        return builder;
    }
    
    /// <summary>
    /// Extension method that enhances logging categories by sql command execution.
    /// </summary>
    public static readonly ILoggerFactory PropertyLoggerFactory =
        LoggerFactory.Create(builder =>
            builder.AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Warning)
                .AddConsole());
}