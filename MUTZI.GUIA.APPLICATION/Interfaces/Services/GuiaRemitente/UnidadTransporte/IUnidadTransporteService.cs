using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.UnidadTransporte;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Transporte
{
    public interface IUnidadTransporteService
    {
        Task<List<UnidadTransporteDTO>> GetAllAsycn();
        Task<List<UnidadTransporteListDTO>> GetListBySuggestion(string numeroDoc);
        Task<PagedResult<UnidadTransporteDTO>> GetListAsync(int pageNumber, int pageSize, IQueryCollection q);
        Task<UnidadTransporteDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(UnidadTransporteCreateDTO createDTO);
        Task<UnidadTransporteDTO> UpdateAsync(int id, UnidadTransporteUpdateDTO updateDTO);
        Task DeleteAsync(int id);
        Task<UpdatedStatusDto> CambiarEstadoAsync(int id, InfoRequestDTO infoRequestDto);
    }
}
