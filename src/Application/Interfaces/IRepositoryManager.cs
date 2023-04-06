namespace Application.Interfaces;

public interface IRepositoryManager
{
    IUserRepository User { get; }
    int SaveChanges();
    Task SaveAsync();
}