using System.Linq.Expressions;

namespace Application.Interfaces;

public interface IRepositoryBase<TEntity>
{
    IQueryable<TEntity> FindAll(bool trackChanges);
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges);
    void Create(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}