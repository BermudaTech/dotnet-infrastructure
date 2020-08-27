using Bermuda.Core.Database.ContextFactory;
using Bermuda.Core.Repository.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Data;

namespace Bermuda.Infrastructure.Database.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly DbContextModel dbContextModel;
        private readonly string httpContextKey = "UnitOfWorkStack";

        private DbContext dbContext;
        private IDbContextTransaction dbContextTransaction;

        public UnitOfWork(
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public UnitOfWork(
            IHttpContextAccessor httpContextAccessor,
            DbContextModel dbContextModel,
            bool isUsedStack)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.dbContextModel = dbContextModel;

            DbContextPrepare(dbContextModel, isUsedStack);
        }

        public void Begin(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            dbContextTransaction = dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            if (dbContext != null)
            {
                dbContext.SaveChanges();
                dbContextTransaction?.Commit();
            }
            else
            {
                GetDbContextFromStack<DbContext>(false).SaveChanges();
            }
        }

        public void Dispose()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
                dbContextTransaction?.Dispose();
            }
            else
            {
                GetDbContextFromStack<DbContext>(true).Dispose();
            }
        }

        public TDbContext GetCurrentDbContext<TDbContext>()
        {
            return (TDbContext)dbContextModel.DbContext;
        }

        internal TDbContext GetDbContextFromStack<TDbContext>(bool isDispose)
        {
            var dbContexts = httpContextAccessor.HttpContext.Items[httpContextKey] as Stack<object>;

            return isDispose ? (TDbContext)dbContexts.Pop() : (TDbContext)dbContexts.Peek();
        }

        private void DbContextPrepare(DbContextModel dbContextModel, bool isUsedStack)
        {
            if (!isUsedStack)
            {
                dbContext = (DbContext)dbContextModel.DbContext;
                return;
            }

            Stack<object> dbContexts = null;
            if (httpContextAccessor.HttpContext.Items.ContainsKey(httpContextKey))
            {
                dbContexts = httpContextAccessor.HttpContext.Items[httpContextKey] as Stack<object>;
            }
            else
            {
                dbContexts = new Stack<object>();
                httpContextAccessor.HttpContext.Items[httpContextKey] = dbContexts;
            }

            dbContexts.Push(dbContextModel.DbContext);
        }
    }
}
