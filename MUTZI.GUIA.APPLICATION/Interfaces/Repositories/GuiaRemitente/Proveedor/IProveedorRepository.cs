namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Proveedor
{
    public interface IProveedorRepository
    {
        Task<List<DOMAIN.Entities.Proveedor>> GetAllAsync();
        Task<DOMAIN.Entities.Proveedor?> GetByIdAsync(int id);
        Task<DOMAIN.Entities.Proveedor?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task CreateAsync(DOMAIN.Entities.Proveedor proveedor);
        Task UpdateAsync(DOMAIN.Entities.Proveedor proveedor);
        Task DeleteAsync(int id);
    }
}
