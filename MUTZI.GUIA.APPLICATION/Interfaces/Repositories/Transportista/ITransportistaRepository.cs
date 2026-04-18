using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.Transportista
{
    public interface ITransportistaRepository
    {
        Task<List<RemiteTransportistum>> GetAllAsync();
        Task<List<RemiteTransportistum>> GetListBySuggestion(string numeroDoc);
        Task<(List<RemiteTransportistum>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO? request);
        Task<RemiteTransportistum?> GetByIdAsync(int id);
        Task CreateAsync(RemiteTransportistum transporte);
        Task UpdateAsync(RemiteTransportistum transporte);
        Task DeleteAsync(int id);
    }
}
