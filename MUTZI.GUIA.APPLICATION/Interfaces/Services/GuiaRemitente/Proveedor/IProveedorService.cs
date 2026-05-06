using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Proveedor;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Proveedor
{
    public interface IProveedorService
    {
        Task<List<ProveedorDTO>> GetAllAsync();
        Task<List<ProveedorListDTO>> GetListBySuggestion(string numeroDoc);
        Task<PagedResult<ProveedorDTO>> GetListAsync(int pageNumber, int pageSize, string? search);
        Task<ProveedorDTO?> GetByIdAsync(int id);
        Task<ProveedorDTO?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task<int> CreateAsync(ProveedorCreateDTO createDTO);
        Task<ProveedorDTO> UpdateAsync(int id, ProveedorUpdateDTO updateDTO);
        Task DeleteAsync(int id);
        Task<EstadoActualizadoResponseDTO> CambiarEstadoAsync(int id, EstadoActualizadoRequestDTO infoRequestDto);
    }
}
