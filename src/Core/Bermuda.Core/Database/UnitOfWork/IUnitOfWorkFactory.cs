using System.Threading.Tasks;

namespace Bermuda.Core.Repository.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        TDbContext GetDbContextFromStack<TDbContext>();

        IUnitOfWork Create();

        IUnitOfWork CreateWithStack();
    }
}
