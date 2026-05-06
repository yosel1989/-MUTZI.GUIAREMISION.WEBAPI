using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Conductor
{
    public interface IConductorService
    {
        Task<List<ConductorDTO>> GetAllAsync();
        Task<List<ConductorListDTO>> GetListBySuggestion(string numeroDoc);
        Task<PagedResult<ConductorDTO>> GetListAsync(int pageNumber, int pageSize, string? search);
        Task<ConductorDTO?> GetByIdAsync(int id);
        Task<ConductorDTO?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task<int> CreateAsync(ConductorCreateDTO createDTO);
        Task<ConductorDTO> UpdateAsync(int id, ConductorUpdateDTO updateDTO);
        Task DeleteAsync(int id);
        Task<EstadoActualizadoResponseDTO> CambiarEstadoAsync(int id, EstadoActualizadoRequestDTO infoRequestDto);
    }
}
