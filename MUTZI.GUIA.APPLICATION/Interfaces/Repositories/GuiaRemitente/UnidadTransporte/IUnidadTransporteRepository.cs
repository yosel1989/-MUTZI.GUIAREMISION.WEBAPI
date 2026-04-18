using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Transporte
{
    public interface IUnidadTransporteRepository
    {
        Task<List<DOMAIN.Entities.UnidadTransporte>> GetAllAsync();
        Task<List<DOMAIN.Entities.UnidadTransporte>> GetListBySuggestion(string numeroDoc);
        Task<(List<DOMAIN.Entities.UnidadTransporte>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO? request);
        Task<DOMAIN.Entities.UnidadTransporte?> GetByIdAsync(int id);
        Task CreateAsync(DOMAIN.Entities.UnidadTransporte transporte);
        Task UpdateAsync(DOMAIN.Entities.UnidadTransporte transporte);
        Task DeleteAsync(int id);
    }
}
