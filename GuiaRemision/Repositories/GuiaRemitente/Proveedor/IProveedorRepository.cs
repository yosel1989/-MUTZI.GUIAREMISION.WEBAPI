namespace GuiaRemision.Repositories.GuiaRemitente.Proveedor
{
    public interface IProveedorRepository
    {
        Task<List<Models.DBMS.Proveedor>> GetAllAsync();
        Task<Models.DBMS.Proveedor?> GetByIdAsync(int id);
        Task<Models.DBMS.Proveedor?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task CreateAsync(Models.DBMS.Proveedor proveedor);
        Task UpdateAsync(Models.DBMS.Proveedor proveedor);
        Task DeleteAsync(int id);
    }
}
