namespace GuiaRemision.Repositories.GuiaRemitente.Conductor
{
    public interface IConductorRepository
    {
        Task<List<Models.DBMS.Conductor>> GetAllAsync();
        Task<Models.DBMS.Conductor?> GetByIdAsync(int id);
        Task CreateAsync(Models.DBMS.Conductor conductor);
        Task UpdateAsync(Models.DBMS.Conductor conductor);
        Task DeleteAsync(int id);
    }
}
