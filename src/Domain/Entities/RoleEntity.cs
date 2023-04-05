using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Security.Claims;
using Domain.Constants.Security.Identity;
using Domain.Extensions;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// Represents the database entity for user roles that inherits from the <see cref="IdentityRole{TKey}"/> model.
/// </summary>
public sealed class RoleEntity : IdentityRole<long>, ICloneable
{
    public static ImmutableArray<string> RoleNames;
    static RoleEntity()
    {
        InitializeMemberRoles();
    }
    
    public async Task<Result<RoleEntity>> AddClaimsAsync(RoleManager<RoleEntity> roleManager, string?[]? claims)
    {
        try
        {
            if (claims == null) return new Result<RoleEntity>(this);
            var roleClaims = await roleManager.GetClaimsAsync(this);
            foreach (var claim in claims)
            {
                if (string.IsNullOrEmpty(claim)) continue;
                if (!RoleClaimEntity.Permissions.Contains(claim, EqualityComparer<string>.Default))
                {
                    return new Result<RoleEntity>(new InvalidOperationException(
                        $"[Class]: '{nameof(RoleEntity)}' [Message]: 'Role claim '{claim}' does not exist in the current context'"));
                }

                if (!roleClaims.Any(c => c.Type == "Permission" && c.Value == claim))
                {
                    await roleManager.AddClaimAsync(this, new Claim("Permission", claim));
                }
            }

            return new Result<RoleEntity>(this);
        }
        catch (Exception exception)
        {
            var msg = string.Format("[Class]: '{0}' [Message]: '{1}'",
                nameof(RoleEntity),
                string.Join(", ", exception.Message));
            
            return new Result<RoleEntity>(new Exception(msg));
        }
    }
    
    public async Task<Result<RoleEntity>> CreateAsync(
        RoleManager<RoleEntity> roleManager)
    {
        try
        {
            var roleEntity = await roleManager.FindByNameAsync(this.Name ?? string.Empty);
            if (roleEntity != null) return new Result<RoleEntity>(this);

            var result = await roleManager.CreateAsync(this);
            if (result.Succeeded)
            {
                return new Result<RoleEntity>(this);
            }

            var errors = result.Errors.Select(error => error.Description).ToList();
            var msg = string.Format("[Class]: '{0}' [Message]: '{1}'",
                nameof(RoleEntity),
                string.Join(", ", errors));
            
            return new Result<RoleEntity>(new Exception(msg));
        }
        catch (Exception exception)
        {
            var msg = string.Format("[Class]: '{0}' [Message]: '{1}'",
                nameof(RoleEntity),
                string.Join(", ", exception.Message));
            
            return new Result<RoleEntity>(new Exception(msg));
        }
    }

    public object Clone() => MemberwiseClone();

    private static void InitializeMemberRoles() => RoleNames = typeof(Roles)
        .FindAllConstantFields()
        .GetConstantFieldsValues();
}