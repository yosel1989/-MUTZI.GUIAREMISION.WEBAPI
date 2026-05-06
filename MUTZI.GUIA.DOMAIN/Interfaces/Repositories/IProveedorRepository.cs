using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IProveedorRepository
    {
        Task<List<Proveedor>> GetAllAsync();
        Task<List<Proveedor>> GetListBySuggestion(string numeroDoc);
        Task<(List<Proveedor>, int)> GetListAsync(int pageNumber, int pageSize, string? search);
        Task<Proveedor?> GetByIdAsync(int id);
        Task<Proveedor?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task CreateAsync(Proveedor proveedor);
        Task<Proveedor> UpdateAsync(Proveedor proveedor);
        Task DeleteAsync(int id);
    }
}
