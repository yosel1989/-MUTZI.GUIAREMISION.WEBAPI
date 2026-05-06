using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IUnidadTransporteService
    {
        Task<List<UnidadTransporteDTO>> GetAllAsycn();
        Task<List<UnidadTransporteListDTO>> GetListBySuggestion(string numeroDoc);
        Task<PagedResult<UnidadTransporteDTO>> GetListAsync(int pageNumber, int pageSize, string? search );
        Task<UnidadTransporteDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(UnidadTransporteCreateDTO createDTO);
        Task<UnidadTransporteDTO> UpdateAsync(int id, UnidadTransporteUpdateDTO updateDTO);
        Task DeleteAsync(int id);
        Task<EstadoActualizadoResponseDTO> CambiarEstadoAsync(int id, EstadoActualizadoRequestDTO infoRequestDto);
    }
}
