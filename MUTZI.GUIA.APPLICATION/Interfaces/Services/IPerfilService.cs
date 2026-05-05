using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Perfil;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IPerfilService
    {
        Task<PerfilDTO?> GetByIdAsync(int id);
        Task<PagedResult<PerfilDTO>> GetListAsync(int pageNumber, int pageSize, string? search);
        Task DeleteAsync(int id);
        Task<UpdatedStatusDto> CambiarEstadoAsync(int id, PerfilUpdateEstadoDTO infoRequestDto);
        Task<int> CreateAsync(PerfilCreateDTO createDTO);
        Task<PerfilDTO> UpdateAsync(int id, PerfilUpdateDTO updateDTO);
    }
}
