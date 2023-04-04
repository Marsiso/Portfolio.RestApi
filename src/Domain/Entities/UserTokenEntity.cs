using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// Represents the database entity and authentication tokens associated with users that inherits from the <see cref="IdentityUserToken{TKey}"/> model.
/// </summary>
public sealed class UserTokenEntity : IdentityUserToken<long>
{
    
}