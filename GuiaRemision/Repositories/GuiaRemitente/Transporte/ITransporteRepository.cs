namespace GuiaRemision.Repositories.GuiaRemitente.Transporte
{
    public interface ITransporteRepository
    {
        Task<List<Models.DBMS.Transporte>> GetAllAsync();
        Task<Models.DBMS.Transporte?> GetByIdAsync(int id);
        Task CreateAsync(Models.DBMS.Transporte transporte);
        Task UpdateAsync(Models.DBMS.Transporte transporte);
        Task DeleteAsync(int id);
    }
}
