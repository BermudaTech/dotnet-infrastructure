using Bermuda.Core.Database.Entity;
using Bermuda.Core.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bermuda.Core.Repository.Repository
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        Task<TEntity> GetByIdAsync(IUnitOfWork unitOfWork, long Id);
        Task<TEntity> GetAsync(IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetListAsync(IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> predicate);
        Task<PagingResponse<TModel>> GetPageAsync<TModel>(IUnitOfWork unitOfWork, IQueryable<TModel> query, PagingRequest request) where TModel : class;
        Task InsertAsync(IUnitOfWork unitOfWork, TEntity entity);
        Task BulkInsertAsync(IUnitOfWork unitOfWork, IEnumerable<TEntity> entities);
        Task UpdateAsync(IUnitOfWork unitOfWork, TEntity entity);
        Task BulkUpdateAsync(IUnitOfWork unitOfWork, IEnumerable<TEntity> entities);
        Task DeleteAsync(IUnitOfWork unitOfWork, TEntity entity);
        Task BulkDeleteAsync(IUnitOfWork unitOfWork, IEnumerable<TEntity> entities);
        Task SoftDeleteAsync(IUnitOfWork unitOfWork, TEntity entity);
        Task BulkSoftDeleteAsync(IUnitOfWork unitOfWork, IEnumerable<TEntity> entities);
    }
}
