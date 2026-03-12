using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Remitente
{
    public interface IRemitenteService
    {
        Task<List<RemitenteListDTO>> GetAllAsync();
        Task<List<RemitenteListDTO>> GetListBySuggestion(string numeroDoc);
        Task<PagedResult<RemitenteDTO>> GetListAsync(int pageNumber, int pageSize);
        Task<RemitenteForGuiaDTO?> GetByIdForGuiaAsync(int id, string tipoGuia);
        Task<RemitenteDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(RemitenteCreateDTO createDTO);
        Task<RemitenteDTO> UpdateAsync(int id, RemitenteUpdateDTO updateDTO);
        Task DeleteAsync(int id);
        Task<UpdatedStatusDto> CancelAsync(int id, InfoRequestDTO infoRequestDto);
    }
}
