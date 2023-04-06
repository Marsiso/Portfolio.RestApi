using System.Linq.Expressions;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Base;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    protected DataContext Context { get; set; }

    protected RepositoryBase(DataContext context)
    {
        Context = context;
    }

    public IQueryable<TEntity> FindAll(bool trackChanges)
        => !trackChanges
            ? Context
                .Set<TEntity>()
                .AsNoTracking()
            : Context
                .Set<TEntity>()
                .AsTracking();

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges)
        => trackChanges
            ? Context.Set<TEntity>()
                .Where(expression)
                .AsNoTracking()
            : Context
                .Set<TEntity>()
                .Where(expression)
                .AsTracking();

    public void Create(TEntity entity) => Context.Set<TEntity>().Add(entity);
    public void Update(TEntity entity) => Context.Set<TEntity>().Remove(entity);
    public void Delete(TEntity entity) => Context.Set<TEntity>().Update(entity);
}