using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// Represents the database entity for user roles that inherits from the <see cref="IdentityRole{TKey}"/> model.
/// </summary>
public sealed class RoleEntity : IdentityRole<long>
{
    
}