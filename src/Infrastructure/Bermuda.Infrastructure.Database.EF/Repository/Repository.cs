using Bermuda.Core.Database.Entity;
using Bermuda.Core.Database.Extensions;
using Bermuda.Core.Repository.Enum;
using Bermuda.Core.Repository.Repository;
using Bermuda.Core.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Bermuda.Infrastructure.Database.Repository
{
    public class Repository<TEntity, PKey> : IRepository<TEntity, PKey>
        where TEntity : EntityBase<PKey>
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public Repository(
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<TEntity> GetAsync(IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var context = unitOfWork.GetCurrentDbContext<DbContext>();

            var dbSet = context.Set<TEntity>();

            return await dbSet.Where(predicate).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TEntity> GetByIdAsync(IUnitOfWork unitOfWork, PKey Id, CancellationToken cancellationToken = default)
        {
            var context = unitOfWork.GetCurrentDbContext<DbContext>();

            var dbSet = context.Set<TEntity>();

            return await dbSet.Where(x => x.Id.Equals(Id)).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var context = unitOfWork.GetCurrentDbContext<DbContext>();

            var dbSet = context.Set<TEntity>();

            return await dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<PagingResponse<TModel>> GetPageAsync<TModel>(IUnitOfWork unitOfWork, IQueryable<TModel> query, PagingRequest request, CancellationToken cancellationToken = default) where TModel : class
        {
            PagingResponse<TModel> response = new PagingResponse<TModel>();

            Expression<Func<TModel, bool>> predicate = null;
            if ((request.Expressions != null) && (request.Expressions.Count > 0))
            {
                predicate = request.Expressions.ToExpression<TModel>();
            }

            if (!request.OrderType.HasValue)
            {
                request.OrderType = OrderType.Asc;
            }

            if (!request.Skip.HasValue)
            {
                request.Skip = 0;
            }

            IQueryable<TModel> source = (predicate != null)
                                      ? query.Where(predicate).AsQueryable<TModel>()
                                      : query.AsQueryable<TModel>();

            response.TotalCount = source.Count<TModel>();

            if (!request.Take.HasValue)
            {
                request.Take = response.TotalCount;
            }

            response.Result = await source.ToOrderBy<TModel>(request.OrderBy, request.OrderType.Value)
                                          .Skip<TModel>(request.Skip.Value)
                                          .Take<TModel>(request.Take.Value).ToListAsync(cancellationToken);

            return response;
        }

        public async Task InsertAsync(IUnitOfWork unitOfWork, TEntity entity, CancellationToken cancellationToken = default)
        {
            var context = unitOfWork.GetCurrentDbContext<DbContext>();

            var dbSet = context.Set<TEntity>();

            await dbSet.AddAsync(entity, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task BulkInsertAsync(IUnitOfWork unitOfWork, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var context = unitOfWork.GetCurrentDbContext<DbContext>();

            var dbSet = context.Set<TEntity>();

            await dbSet.AddRangeAsync(entities, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(IUnitOfWork unitOfWork, TEntity entity, CancellationToken cancellationToken = default)
        {
            var context = unitOfWork.GetCurrentDbContext<DbContext>();

            if (entity is EntityBaseAudit<PKey> auditEntity)
            {
                auditEntity.UpdatedDate ??= DateTime.UtcNow;
            }

            context.Entry(entity).State = EntityState.Modified;

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task BulkUpdateAsync(IUnitOfWork unitOfWork, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var context = unitOfWork.GetCurrentDbContext<DbContext>();

            foreach (var entity in entities)
            {
                if (entity is EntityBaseAudit<PKey> auditEntity)
                {
                    auditEntity.UpdatedDate ??= DateTime.UtcNow;
                }
            }

            context.UpdateRange(entities);

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(IUnitOfWork unitOfWork, TEntity entity, CancellationToken cancellationToken = default)
        {
            var context = unitOfWork.GetCurrentDbContext<DbContext>();

            context.Remove(entity);

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task BulkDeleteAsync(IUnitOfWork unitOfWork, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var context = unitOfWork.GetCurrentDbContext<DbContext>();

            context.RemoveRange(entities);

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task SoftDeleteAsync(IUnitOfWork unitOfWork, TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity is not EntityBaseAudit<PKey> entityBase)
            {
                throw new InvalidOperationException($"SoftDeleteAsync requires entity of type EntityBaseAudit<{typeof(PKey).Name}>. Actual type: {entity.GetType().Name}");
            }

            var context = unitOfWork.GetCurrentDbContext<DbContext>();

            entityBase.StatusType = StatusType.Deleted;
            entityBase.UpdatedDate ??= DateTime.UtcNow;

            context.Entry(entity).State = EntityState.Modified;

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task BulkSoftDeleteAsync(IUnitOfWork unitOfWork, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var context = unitOfWork.GetCurrentDbContext<DbContext>();

            foreach (var entity in entities)
            {
                if (entity is not EntityBaseAudit<PKey> entityBase)
                {
                    throw new InvalidOperationException($"BulkSoftDeleteAsync requires entities of type EntityBaseAudit<{typeof(PKey).Name}>. Actual type: {entity.GetType().Name}");
                }

                entityBase.StatusType = StatusType.Deleted;
                entityBase.UpdatedDate ??= DateTime.UtcNow;
            }

            context.UpdateRange(entities);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
