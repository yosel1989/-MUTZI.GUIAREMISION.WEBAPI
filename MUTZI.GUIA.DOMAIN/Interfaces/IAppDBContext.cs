namespace MUTZI.GUIA.DOMAIN.Interfaces
{
    public interface IAppDBContext
    {
        Task BeginTransactionAsync();

        Task CommitAsync();

        Task RollbackAsync();
    }
}
