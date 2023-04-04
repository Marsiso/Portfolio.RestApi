using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// Represents the database entity and claims associated with user roles that inherits from the <see cref="IdentityRoleClaim{TKey}"/> model.
/// </summary>
public sealed class RoleClaimEntity : IdentityRoleClaim<long>
{
    
}