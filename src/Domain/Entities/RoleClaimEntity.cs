using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Security.Claims;
using Domain.Constants.Security.Identity;
using Domain.Extensions;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// Represents the database entity and claims associated with user roles that inherits from the <see cref="IdentityRoleClaim{TKey}"/> model.
/// </summary>
public sealed class RoleClaimEntity : IdentityRoleClaim<long>
{
    public static ImmutableArray<string> Permissions;

    static RoleClaimEntity()
    {
        InitializePermissions();
    }
    
    private static void InitializePermissions() => Permissions = typeof(Permissions)
        .FindAllConstantFields()
        .GetConstantFieldsValues();
}