using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.UnidadTransporte;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Transporte
{
    public interface ITransporteService
    {
        Task<List<UnidadTransporteDTO>> GetAllAsycn();
        Task<List<UnidadTransporteDTO>> GetListByName(string numeroDoc, string nombre);
        Task<PagedResult<UnidadTransporteDTO>> GetListAsync(int pageNumber, int pageSize);
        Task<UnidadTransporteDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(UnidadTransporteCreateDTO createDTO);
        Task<UnidadTransporteDTO> UpdateAsync(int id, UnidadTransporteUpdateDTO updateDTO);
        Task DeleteAsync(int id);
        Task CancelAsync(int id, int status);
    }
}
