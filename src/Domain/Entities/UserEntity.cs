﻿using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// Represents the database entity and the application user that inherits from the <see cref="IdentityUser{TKey}"/> model.
/// </summary>
public sealed class UserEntity : IdentityUser<long>, ICloneable
{
    public object Clone() => this.MemberwiseClone();
}