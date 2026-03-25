using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Proveedor;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Proveedor
{
    public interface IProveedorService
    {
        Task<List<ProveedorDTO>> GetAllAsync();
        Task<List<ProveedorListDTO>> GetListBySuggestion(string numeroDoc);
        Task<PagedResult<ProveedorDTO>> GetListAsync(int pageNumber, int pageSize, IQueryCollection q);
        Task<ProveedorDTO?> GetByIdAsync(int id);
        Task<ProveedorDTO?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task<int> CreateAsync(ProveedorCreateDTO createDTO);
        Task<ProveedorDTO> UpdateAsync(int id, ProveedorUpdateDTO updateDTO);
        Task DeleteAsync(int id);
        Task<UpdatedStatusDto> CambiarEstadoAsync(int id, InfoRequestDTO infoRequestDto);
    }
}
