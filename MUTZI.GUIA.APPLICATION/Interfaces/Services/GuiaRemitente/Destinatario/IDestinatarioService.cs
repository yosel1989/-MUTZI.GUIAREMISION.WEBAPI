using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Destinatario
{
    public interface IDestinatarioService
    {
        Task<List<DestinatarioDTO>> GetAllAsync();
        Task<PagedResult<DestinatarioDTO>> GetListAsync(int pageNumber, int pageSize);
        Task<DestinatarioDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(DestinatarioCreateDTO createDTO);
        Task<DestinatarioDTO> UpdateAsync(int id, DestinatarioUpdateDTO updateDTO);
        Task DeleteAsync(int id);
    }
}
