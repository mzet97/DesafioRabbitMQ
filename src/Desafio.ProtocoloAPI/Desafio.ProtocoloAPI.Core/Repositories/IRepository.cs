using Desafio.ProtocoloAPI.Core.Entities;
using Desafio.ProtocoloAPI.Core.Models;
using System.Linq.Expressions;

namespace Desafio.ProtocoloAPI.Core.Repositories;

public interface IRepository<TEntity> : IDisposable where TEntity : IEntityBase
{
    Task Add(TEntity entity);

    Task<TEntity?> GetById(Guid id);

    Task<IEnumerable<TEntity>> GetAll();

    Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);

    Task Update(TEntity entity);

    Task Remove(Guid id);
    Task RemoveAll();

    Task<BaseResult<TEntity>> Search(
         Expression<Func<TEntity, bool>>? predicate = null,
         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
         int pageSize = 10, int page = 1);
}
