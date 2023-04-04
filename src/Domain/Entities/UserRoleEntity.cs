using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// Represents the database entity and links user roles with users, inherits from the <see cref="IdentityUserRole{TKey}"/> model.
/// </summary>
public sealed class UserRoleEntity : IdentityUserRole<long>
{
}