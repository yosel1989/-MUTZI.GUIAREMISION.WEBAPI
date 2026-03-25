using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Remitente
{
    public interface IRemitenteRepository
    {
        Task<List<DOMAIN.Entities.Remitente>> GetAllAsync();
        Task<List<DOMAIN.Entities.Remitente>> GetOnlyNameAsync();
        Task<List<DOMAIN.Entities.Remitente>> GetListBySuggestion(string numeroDoc);
        Task<(List<DOMAIN.Entities.Remitente>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO? request);
        Task<DOMAIN.Entities.Remitente?> GetByIdAsync(int id);
        Task<bool> GetBySerieAsync(string ruc, string serie);
        Task CreateAsync(DOMAIN.Entities.Remitente remitente);
        Task UpdateAsync(DOMAIN.Entities.Remitente remitente);
        Task DeleteAsync(int id);
        Task<DOMAIN.Entities.GuiaRemision?> GetLastBySerieAsync(string serie);
    }
}
