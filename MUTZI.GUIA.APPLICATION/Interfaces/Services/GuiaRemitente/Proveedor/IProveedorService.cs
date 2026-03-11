using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Proveedor;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Proveedor
{
    public interface IProveedorService
    {
        Task<List<ProveedorDTO>> GetAllAsync();
        Task<List<ProveedorListDTO>> GetListByName(string numeroDoc, string nombre);
        Task<PagedResult<ProveedorDTO>> GetListAsync(int pageNumber, int pageSize);
        Task<ProveedorDTO?> GetByIdAsync(int id);
        Task<ProveedorDTO?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task<int> CreateAsync(ProveedorCreateDTO createDTO);
        Task<ProveedorDTO> UpdateAsync(int id, ProveedorUpdateDTO updateDTO);
        Task DeleteAsync(int id);
        Task CancelAsync(int id, int status);
    }
}
