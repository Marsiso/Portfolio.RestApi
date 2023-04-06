using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data.Repositories.Base;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public sealed class UserRepository : RepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository(DataContext context) : base(context)
    {
    }

    public async Task<UserEntity?> GetByUserNameAsync(string userName, bool trackChanges = false)
    {
        if (string.IsNullOrEmpty(userName)) return null;
        return await FindByCondition(
                user => userName == user.UserName,
                trackChanges)
            .SingleOrDefaultAsync();
    }

    public async Task<UserEntity?> GetByIdAsync(long userId, bool trackChanges = false) =>
        await FindByCondition(user => user.Id == userId, trackChanges)
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<UserEntity>> GetAllAsync(bool trackChanges = false) =>
        await FindAll(trackChanges)
            .ToListAsync();

    public new void Create(UserEntity user) => base.Create(user);

    public new void Update(UserEntity user) => base.Update(user);

    public new void Delete(UserEntity user) => base.Delete(user);
}