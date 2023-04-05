using System.Diagnostics.Contracts;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// Represents the database entity and the application user that inherits from the <see cref="IdentityUser{TKey}"/> model.
/// </summary>
public sealed class UserEntity : IdentityUser<long>, ICloneable
{
    public async Task<Result<UserEntity>> CreateAsync(
        string userPassword,
        UserManager<UserEntity> userManager)
    {
        try
        {
            var userEntity = await userManager.FindByEmailAsync(this.Email ?? string.Empty);
            if (userEntity != null) return new Result<UserEntity>(userEntity);

            var result = await userManager.CreateAsync(this, userPassword);
            if (result.Succeeded)
            {
                return new Result<UserEntity>(this);
            }

            var errors = result.Errors.Select(error => error.Description).ToList();
            var msg = string.Format("[Class]: '{0}' [Message]: '{1}'",
                nameof(UserEntity),
                string.Join(", ", errors));
            
            return new Result<UserEntity>(new Exception(msg));
        }
        catch (Exception exception)
        {
            var msg = string.Format("[Class]: '{0}' [Message]: '{1}'",
                nameof(UserEntity),
                string.Join(", ", exception.Message));
            
            return new Result<UserEntity>(new Exception(msg));
        }
    }
    
    public async Task<Result<UserEntity>> AssignRolesAsync(
        string?[]? roles,
        UserManager<UserEntity> userManager)
    {
        try
        {
            if (roles == null)
            {
                return new Result<UserEntity>(this);
            }

            foreach (var role in roles)
            {
                if (string.IsNullOrEmpty(role)) continue;
                if (!RoleEntity.RoleNames.Contains(role, EqualityComparer<string>.Default))
                {
                    return new Result<UserEntity>(new InvalidOperationException(
                        $"[Class]: '{nameof(UserEntity)}' [Message]: 'Role '{role}' does not exist in the current context'"));
                }

                if (await userManager.IsInRoleAsync(this, role)) continue;
                var result = await userManager.AddToRoleAsync(this, role);
                
                if (result.Succeeded) return new Result<UserEntity>(this);
                var msg = string.Format("[Class]: '{0}' [Message]: 'Role '{1}' failed to be assigned to user with ID '{2}' and username '{3}''",
                    nameof(UserEntity), 
                    role,
                    this.Id.ToString(),
                    this.UserName);
                
                return new Result<UserEntity>(new Exception(msg));
            }
            
            return new Result<UserEntity>(this);
        }
        catch (Exception exception)
        {
            var msg = string.Format("[Class]: '{0}' [Message]: '{1}'",
                nameof(UserEntity),
                string.Join(", ", exception.Message));
            
            return new Result<UserEntity>(new Exception(msg));
        }
    }

    public object Clone() => this.MemberwiseClone();
}