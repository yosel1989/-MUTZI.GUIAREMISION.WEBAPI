using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Destinatario
{
    public interface IDestinatarioService
    {
        Task<List<DestinatarioDTO>> GetAllAsync();
        Task<List<DestinatarioListDTO>> GetListBySuggestion(string numeroDoc);
        Task<PagedResult<DestinatarioDTO>> GetListAsync(int pageNumber, int pageSize, IQueryCollection q);
        Task<DestinatarioDTO?> GetByIdAsync(int id);
        Task<DestinatarioDTO?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task<int> CreateAsync(DestinatarioCreateDTO createDTO);
        Task<DestinatarioDTO> UpdateAsync(int id, DestinatarioUpdateDTO updateDTO);
        Task DeleteAsync(int id);
        Task<UpdatedStatusDto> CambiarEstadoAsync(int id, InfoRequestDTO infoRequestDto);
    }
}
