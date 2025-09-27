using GuiaRemision.DTOs.GuiaRemitente.Proveedor;

namespace GuiaRemision.Services.GuiaRemitente.Proveedor
{
    public interface IProveedorService
    {
        Task<List<ProveedorDTO>> GetAllAsync();
        Task<ProveedorDTO?> GetByIdAsync(int id);
        Task<ProveedorDTO?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task<int> CreateAsync(ProveedorCreateDTO createDTO);
        Task<ProveedorUpdateDTO> UpdateAsync(int id, ProveedorUpdateDTO updateDTO);
        Task DeleteAsync(int id);
    }
}
