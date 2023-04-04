using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// Represents the database entity and associates users with logins, inherits from the <see cref="IdentityUserLogin{TKey}"/> model.
/// </summary>
public sealed class UserLoginEntity : IdentityUserLogin<long>
{
    
}