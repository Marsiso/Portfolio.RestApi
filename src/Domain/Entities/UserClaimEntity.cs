using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// Represents the database entity and claims associated with users that inherits from the <see cref="IdentityUserClaim{TKey}"/> model.
/// </summary>
public sealed class UserClaimEntity : IdentityUserClaim<long>
{
    
}