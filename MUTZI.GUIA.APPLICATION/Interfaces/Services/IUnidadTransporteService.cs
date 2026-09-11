using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IUnidadTransporteService
    {
        Task<List<TransportUnitDTO>> GetAllAsycn();
        Task<List<TransportUnitListDTO>> GetListBySuggestion(string numeroDoc);
        Task<PagedResult<TransportUnitDTO>> GetListAsync(int pageNumber, int pageSize, string? search );
        Task<TransportUnitDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(TransportUnitCreateDTO createDTO);
        Task<TransportUnitDTO> UpdateAsync(int id, TransportUnitUpdateDTO updateDTO);
        Task DeleteAsync(int id);
        Task<EstadoActualizadoResponseDTO> CambiarEstadoAsync(int id, EstadoActualizadoRequestDTO infoRequestDto);
    }
}
