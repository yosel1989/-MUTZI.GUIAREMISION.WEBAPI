using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Conductor;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Conductor
{
    public interface IConductorService
    {
        Task<List<ConductorDTO>> GetAllAsync();
        Task<List<ConductorDTO>> GetListByName(string numeroDoc, string nombre);
        Task<PagedResult<ConductorDTO>> GetListAsync(int pageNumber, int pageSize);
        Task<ConductorDTO?> GetByIdAsync(int id);
        Task<ConductorDTO?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task<int> CreateAsync(ConductorCreateDTO createDTO);
        Task<ConductorDTO> UpdateAsync(int id, ConductorUpdateDTO updateDTO);
        Task DeleteAsync(int id);
        Task CancelAsync(int id, int status);
    }
}
