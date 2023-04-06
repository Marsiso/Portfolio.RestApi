using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public sealed class IdentityManager : IIdentityManager
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<RoleEntity> _roleManager;
    private readonly ILogger<IdentityManager> _logger;

    public IdentityManager(UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager,
        ILogger<IdentityManager> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<bool> TryCreateUserAsync([NotNullWhen(true)] UserEntity? user,
        [NotNullWhen(true)] string? password)
    {
        try
        {
            if (user?.Email == null)
            {
                const string warn = $"[Service]: '{nameof(IdentityManager)}'" +
                                    "[Message]: 'User email address cannot be a null reference object or an empty string'";
                _logger.LogWarning(warn);
                return false;
            }

            var userEntity = await _userManager.FindByEmailAsync(user.Email);
            if (userEntity != null)
            {
                _logger.LogInformation("[Service]: '{Service}' [Message]: 'User with email address '{Email}' already exists in the persistence store'",
                    nameof(IdentityManager),
                    userEntity.Email);
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                const string warn = $"[Service]: '{nameof(IdentityManager)}'" +
                                    "[Message]: 'User password cannot be a null reference object or an empty string'";
                _logger.LogWarning(warn);
                return false;
            }

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                _logger.LogInformation("[Service]: '{Service}' [Message]: 'User with email address '{Email}' successfully created in the persistence store'",
                    nameof(IdentityManager),
                    user.Email);
                return true;
            }

            _logger.LogWarning("[Service]: '{Service}' [Message]: 'User with email address '{Email}' failed to be created in the persistence store'",
                nameof(IdentityManager),
                user.Email);
            
            return false;
        }
        catch (Exception exception)
        {
            _logger.LogError("[Service]: '{Service}' [Message]: '{Message}'",
                nameof(IdentityManager),
                exception.Message);
        }

        return false;
    }

    public async Task<bool> TryAssignRolesAsync([NotNullWhen(true)] UserEntity? user,
        [NotNullWhen(true)] string?[]? roles)
    {
        try
        {
            if (user?.Email == null)
            {
                const string warn = $"[Service]: '{nameof(IdentityManager)}'" +
                                    "[Message]: 'User email address cannot be a null reference object or an empty string'";
                _logger.LogWarning(warn);
                return false;
            }

            if (roles == null)
            {
                const string warn = $"[Service]: '{nameof(IdentityManager)}'" +
                                    "[Message]: 'Roles cannot be a null reference object'";
                _logger.LogWarning(warn);
                return false;
            }

            var userEntity = await _userManager.FindByEmailAsync(user.Email);
            if (userEntity == null)
            {
                _logger.LogWarning("[Service]: '{Service}' [Message]: 'User with email address '{Email}' does not exist in the persistence store'",
                    nameof(IdentityManager),
                    user.Email);
                return false;
            }

            foreach (var role in roles)
            {
                if (string.IsNullOrEmpty(role))
                {
                    const string warn = $"[Service]: '{nameof(IdentityManager)}'" +
                                        "[Message]: 'Role name cannot be a null reference object or an empty string'";
                    _logger.LogWarning(warn);
                    return false;
                }

                if (!await _roleManager.RoleExistsAsync(role))
                {
                    _logger.LogWarning("[Service]: '{Service}' [Message]: 'Role with name '{Name}' does not exist in the persistence store'",
                        nameof(IdentityManager),
                        role);
                    return false;
                }

                var result = await _userManager.AddToRoleAsync(userEntity, role);
                if (result.Succeeded)
                {
                    _logger.LogInformation("[Service]: '{Service}' [Message]: 'Role '{Role}' successfully assigned to user with email address '{Email}' in the persistence store'",
                        nameof(IdentityManager),
                        role,
                        user.Email);
                    continue;
                }
                
                _logger.LogInformation("[Service]: '{Service}' [Message]: 'Role '{Role}' failed to be assigned to user with email address '{Email}' in the persistence store'",
                    nameof(IdentityManager),
                    role,
                    user.Email);

                return false;
            }

            return true;
        }
        catch (Exception exception)
        {
            _logger.LogError("[Service]: '{Service}' [Message]: '{Message}'",
                nameof(IdentityManager),
                exception.Message);
            return false;
        }
    }

    public async Task<bool> TryAddRoleClaimsAsync([NotNullWhen(true)] RoleEntity? role,
        [NotNullWhen(true)] string? claimType, [NotNullWhen(true)] string?[]? claims)
    {
        try
        {
            if (role?.Name == null)
            {
                const string warn = $"[Service]: '{nameof(IdentityManager)}'" +
                                    "[Message]: 'Role name cannot be a null reference object or an empty string'";
                _logger.LogWarning(warn);
                return false;
            }
            
            if (claims == null)
            {
                const string warn = $"[Service]: '{nameof(IdentityManager)}'" +
                                    "[Message]: 'Claim values cannot be a null reference object'";
                _logger.LogWarning(warn);
                return false;
            }
            
            if (string.IsNullOrEmpty(claimType))
            {
                const string warn = $"[Service]: '{nameof(IdentityManager)}'" +
                                    "[Message]: 'Claim type cannot be a null reference object or an empty string'";
                _logger.LogWarning(warn);
                return false;
            }

            var roleEntity = await _roleManager.FindByNameAsync(role.Name);
            if (roleEntity == null)
            {
                _logger.LogWarning("[Service]: '{Service}' [Message]: 'Role with name '{Name}' does not exist in the persistence store'",
                    nameof(IdentityManager),
                    role.Name);
                return false;
            }

            foreach (var claim in claims)
            {
                if (string.IsNullOrEmpty(claim))
                {
                    const string warn = $"[Service]: '{nameof(IdentityManager)}'" +
                                        "[Message]: 'Claim value cannot be a null reference object or an empty string'";
                    _logger.LogWarning(warn);
                    return false;
                }

                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    _logger.LogWarning("[Service]: '{Service}' [Message]: 'Role with name '{Name}' does not exist in the persistence store'",
                        nameof(IdentityManager),
                        role);
                    return false;
                }

                var result = await _roleManager.AddClaimAsync(roleEntity, new Claim(claimType, claim));
                if (result.Succeeded)
                {
                    _logger.LogInformation("[Service]: '{Service}' [Message]: 'Role claim '{Claim}' with value '{ClaimValue}' successfully assigned to role '{Role}' in the persistence store'",
                        nameof(IdentityManager),
                        claimType,
                        claim,
                        role);
                    continue;
                }

                _logger.LogWarning("[Service]: '{Service}' [Message]: 'Role claim '{Claim}' with value '{ClaimValue}' failed to be assigned to role '{Role}' in the persistence store'",
                    nameof(IdentityManager),
                    claimType,
                    claim,
                    role);
                
                return false;
            }

            return true;
        }
        catch (Exception exception)
        {
            _logger.LogError("[Service]: '{Service}' [Message]: '{Message}'",
                nameof(IdentityManager),
                exception.Message);
            return false;
        }
    }

    public async Task<bool> TryCreateRoleAsync([NotNullWhen(true)] RoleEntity? role)
    {
        try
        {
            if (role?.Name == null)
            {
                const string warn = $"[Service]: '{nameof(IdentityManager)}'" +
                                    "[Message]: 'Role name cannot be a null reference object or an empty string'";
                _logger.LogWarning(warn);
                return false;
            }

            var roleEntity = await _roleManager.FindByNameAsync(role.Name);
            if (roleEntity != null)
            {
                _logger.LogInformation("[Service]: '{Service}' [Message]: 'Role '{Name}' already exists in the persistence store'",
                    nameof(IdentityManager),
                    roleEntity.Name);
                return false;
            }

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                _logger.LogInformation("[Service]: '{Service}' [Message]: 'Role '{Name}' successfully created in the persistence store'",
                    nameof(IdentityManager),
                    role.Name);
                return true;
            }
            
            _logger.LogInformation("[Service]: '{Service}' [Message]: 'Role '{Name}' failed to be created in the persistence store'",
                nameof(IdentityManager),
                role.Name);

            return false;
        }
        catch (Exception exception)
        {
            _logger.LogError("[Service]: '{Service}' [Message]: '{Message}'",
                nameof(IdentityManager),
                exception.Message);
            return false;
        }
    }
}