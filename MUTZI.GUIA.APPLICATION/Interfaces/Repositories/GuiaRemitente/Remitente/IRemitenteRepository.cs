namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Remitente
{
    public interface IRemitenteRepository
    {
        Task<List<DOMAIN.Entities.Remitente>> GetAllAsync();
        Task<(List<DOMAIN.Entities.Remitente>, int)> GetListAsync(int pageNumber, int pageSize);
        Task<DOMAIN.Entities.Remitente?> GetByIdAsync(int id);
        Task CreateAsync(DOMAIN.Entities.Remitente remitente);
        Task UpdateAsync(DOMAIN.Entities.Remitente remitente);
        Task DeleteAsync(int id);
        Task<DOMAIN.Entities.GuiaRemision?> GetLastBySerieAsync(string serie);
    }
}
