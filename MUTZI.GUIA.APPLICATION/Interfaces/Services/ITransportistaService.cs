using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Transportista;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface ITransportistaService
    {
        Task<List<TransportistaDTO>> GetAllAsycn();
        Task<List<TransportistaDTO>> GetListBySuggestion(string numeroDoc);
        Task<PagedResult<TransportistaDTO>> GetListAsync(int pageNumber, int pageSize, string? search);
        Task<TransportistaDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(TransportistaCreateDTO createDTO);
        Task<TransportistaDTO> UpdateAsync(int id, TransportistaUpdateDTO updateDTO);
        Task DeleteAsync(int id);
        Task<EstadoActualizadoResponseDTO> CambiarEstadoAsync(int id, EstadoActualizadoRequestDTO infoRequestDto);
    }
}
