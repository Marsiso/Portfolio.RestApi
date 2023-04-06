using Application.Interfaces;

namespace Infrastructure.Data.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly DataContext _context;
    private IUserRepository? _user;
    
    public IUserRepository User => _user ??= new UserRepository(_context);
    
    public RepositoryManager(DataContext context)
    {
        _context = context;
    }

    public int SaveChanges() => _context.SaveChanges();
    public async Task SaveAsync() => await _context.SaveChangesAsync();
}