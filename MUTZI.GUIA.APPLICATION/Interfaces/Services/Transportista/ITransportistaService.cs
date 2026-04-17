using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.UnidadTransporte;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.RemiteTransportista;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.Transportista
{
    public interface ITransportistaService
    {
        Task<List<RemiteTransportistaDTO>> GetAllAsycn();
        Task<List<RemiteTransportistaDTO>> GetListBySuggestion(string numeroDoc);
        Task<PagedResult<RemiteTransportistaDTO>> GetListAsync(int pageNumber, int pageSize, IQueryCollection q);
        Task<RemiteTransportistaDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(RemiteTransportistaCreateDTO createDTO);
        Task<RemiteTransportistaDTO> UpdateAsync(int id, RemiteTransportistaUpdateDTO updateDTO);
        Task DeleteAsync(int id);
        Task<UpdatedStatusDto> CambiarEstadoAsync(int id, InfoRequestDTO infoRequestDto);
    }
}
