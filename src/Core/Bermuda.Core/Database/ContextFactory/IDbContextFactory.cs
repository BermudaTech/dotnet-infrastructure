namespace Bermuda.Core.Database.ContextFactory
{
    public interface IDbContextFactory
    {
        DbContextModel GetDbContextModel();
    }
}
