using Bermuda.Core.Database.Entity;
using Bermuda.Core.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Bermuda.Core.Repository.Repository
{
    public interface IRepository<TEntity, PKey> where TEntity : EntityBase<PKey>
    {
        Task<TEntity> GetByIdAsync(IUnitOfWork unitOfWork, PKey Id, CancellationToken cancellationToken = default);
        Task<TEntity> GetAsync(IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetListAsync(IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<PagingResponse<TModel>> GetPageAsync<TModel>(IUnitOfWork unitOfWork, IQueryable<TModel> query, PagingRequest request, CancellationToken cancellationToken = default) where TModel : class;
        Task InsertAsync(IUnitOfWork unitOfWork, TEntity entity, CancellationToken cancellationToken = default);
        Task BulkInsertAsync(IUnitOfWork unitOfWork, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task UpdateAsync(IUnitOfWork unitOfWork, TEntity entity, CancellationToken cancellationToken = default);
        Task BulkUpdateAsync(IUnitOfWork unitOfWork, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task DeleteAsync(IUnitOfWork unitOfWork, TEntity entity, CancellationToken cancellationToken = default);
        Task BulkDeleteAsync(IUnitOfWork unitOfWork, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(IUnitOfWork unitOfWork, TEntity entity, CancellationToken cancellationToken = default);
        Task BulkSoftDeleteAsync(IUnitOfWork unitOfWork, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    }
}
