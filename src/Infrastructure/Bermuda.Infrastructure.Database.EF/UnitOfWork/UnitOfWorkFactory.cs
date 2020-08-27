using Bermuda.Core.Database.ContextFactory;
using Bermuda.Core.Repository.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace Bermuda.Infrastructure.Database.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IDbContextFactory dbContextFactory;

        public UnitOfWorkFactory(
            IHttpContextAccessor httpContextAccessor,
            IDbContextFactory dbContextFactory)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.dbContextFactory = dbContextFactory;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(httpContextAccessor, dbContextFactory.GetDbContextModel(), false);
        }

        public IUnitOfWork CreateWithStack()
        {
            return new UnitOfWork(httpContextAccessor, dbContextFactory.GetDbContextModel(), true);
        }

        public TDbContext GetDbContextFromStack<TDbContext>()
        {
            return new UnitOfWork(httpContextAccessor).GetDbContextFromStack<TDbContext>(false);
        }
    }
}
