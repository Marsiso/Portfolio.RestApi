using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IIdentityManager
{
     Task<bool> TryCreateUserAsync([NotNullWhen(true)] UserEntity? user,
          [NotNullWhen(true)] string? password);
     Task<bool> TryAssignRolesAsync([NotNullWhen(true)] UserEntity? user,
          [NotNullWhen(true)] string?[]? roles);
     Task<bool> TryAddRoleClaimsAsync([NotNullWhen(true)] RoleEntity? role,
          [NotNullWhen(true)] string? claimType, [NotNullWhen(true)] string?[]? claims);
     Task<bool> TryCreateRoleAsync([NotNullWhen(true)] RoleEntity? role);
}