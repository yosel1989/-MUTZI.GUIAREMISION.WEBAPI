namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Proveedor
{
    public interface IProveedorRepository
    {
        Task<List<DOMAIN.Entities.Proveedor>> GetAllAsync();
        Task<List<DOMAIN.Entities.Proveedor>> GetListByName(string numeroDoc, string nombre);
        Task<(List<DOMAIN.Entities.Proveedor>, int)> GetListAsync(int pageNumber, int pageSize);
        Task<DOMAIN.Entities.Proveedor?> GetByIdAsync(int id);
        Task<DOMAIN.Entities.Proveedor?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task CreateAsync(DOMAIN.Entities.Proveedor proveedor);
        Task UpdateAsync(DOMAIN.Entities.Proveedor proveedor);
        Task DeleteAsync(int id);
    }
}
