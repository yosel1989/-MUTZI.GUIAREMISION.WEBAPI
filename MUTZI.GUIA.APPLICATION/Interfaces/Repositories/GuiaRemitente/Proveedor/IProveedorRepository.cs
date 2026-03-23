using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Proveedor
{
    public interface IProveedorRepository
    {
        Task<List<DOMAIN.Entities.Proveedor>> GetAllAsync();
        Task<List<DOMAIN.Entities.Proveedor>> GetListBySuggestion(string numeroDoc);
        Task<(List<DOMAIN.Entities.Proveedor>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO? request);
        Task<DOMAIN.Entities.Proveedor?> GetByIdAsync(int id);
        Task<DOMAIN.Entities.Proveedor?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task CreateAsync(DOMAIN.Entities.Proveedor proveedor);
        Task UpdateAsync(DOMAIN.Entities.Proveedor proveedor);
        Task DeleteAsync(int id);
    }
}
