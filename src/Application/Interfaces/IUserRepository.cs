using Domain.Entities;
using LanguageExt.Common;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<UserEntity?> GetByUserNameAsync(string userName, bool trackChanges);
    Task<UserEntity?> GetByIdAsync(long userId, bool trackChanges);
    Task<IEnumerable<UserEntity>> GetAllAsync(bool trackChanges);
    void Create(UserEntity user);
    void Update(UserEntity user);
    void Delete(UserEntity user);
}