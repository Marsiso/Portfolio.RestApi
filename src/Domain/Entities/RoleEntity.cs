﻿using System.Collections.Immutable;
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
    
    public async Task<Result<RoleEntity>> CreateAsync(
        RoleManager<RoleEntity> roleManager)
    {
        try
        {
            var roleEntity = await roleManager.FindByNameAsync(this.Name ?? string.Empty);
            if (roleEntity != null) return new Result<RoleEntity>(this);

            var result = await roleManager.CreateAsync(this);
            if (result.Succeeded) return new Result<RoleEntity>(this);

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
                exception);
            
            return new Result<RoleEntity>(new Exception(msg));
        }
    }

    public object Clone() => MemberwiseClone();

    private static void InitializeMemberRoles() => RoleNames = typeof(Roles)
        .FindAllConstantFields()
        .GetConstantFieldsValues();
}