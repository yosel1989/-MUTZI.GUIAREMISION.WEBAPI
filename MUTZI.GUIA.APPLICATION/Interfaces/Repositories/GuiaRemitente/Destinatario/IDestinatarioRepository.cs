using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Destinatario
{
    public interface IDestinatarioRepository
    {
        Task<List<DOMAIN.Entities.Destinatario>> GetAllAsync();
        Task<List<DOMAIN.Entities.Destinatario>> GetListBySuggestion(string numeroDoc);
        Task<(List<DOMAIN.Entities.Destinatario>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO? request);
        Task<DOMAIN.Entities.Destinatario?> GetByIdAsync(int id);
        Task CreateAsync(DOMAIN.Entities.Destinatario destinatario);
        Task UpdateAsync(DOMAIN.Entities.Destinatario destinatario);
        Task DeleteAsync(int id);
    }
}
