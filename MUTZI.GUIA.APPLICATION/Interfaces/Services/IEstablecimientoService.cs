using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IEstablecimientoService
    {
        Task<List<EstablecimientoListDTO>> GetAllByRucToSelectAsync(string ruc);
        Task<List<EstablecimientoListToModalDTO>> GetListBySuggestionAsync(string ruc, string? search);
        Task<EstablecimientoDTO?> GetByIdAsync(int id);
        Task<PagedResult<EstablecimientoDTO>> GetListAsync(int pageNumber, int pageSize, string? search);
        Task DeleteAsync(int id);
        Task<EstadoActualizadoResponseDTO> CambiarEstadoAsync(int id, EstadoActualizadoRequestDTO infoRequestDto);
        Task<int> CreateAsync(EstablecimientoCreateDTO createDTO);

        Task<EstablecimientoDTO> UpdateAsync(int id, EstablecimientoUpdateDTO updateDTO);
        Task<EstablecimientoRemitenteDTO?> GetByIdForGuiaAsync(int id, string tipoGuia);
    }
}
