namespace GuiaRemision.Repositories.GuiaRemitente.Remitente
{
    public interface IRemitenteRepository
    {
        Task<List<Models.DBMS.Remitente>> GetAllAsync();
        Task<Models.DBMS.Remitente?> GetByIdAsync(int id);
        Task CreateAsync(Models.DBMS.Remitente remitente);
        Task UpdateAsync(Models.DBMS.Remitente remitente);
        Task DeleteAsync(int id);
    }
}
