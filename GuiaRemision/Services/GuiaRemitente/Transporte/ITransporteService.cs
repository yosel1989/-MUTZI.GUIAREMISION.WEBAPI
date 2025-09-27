using GuiaRemision.DTOs.GuiaRemitente.UnidadTransporte;

namespace GuiaRemision.Services.GuiaRemitente.Transporte
{
    public interface ITransporteService
    {
        Task<List<UnidadTransporteDTO>> GetAllAsycn();
        Task<UnidadTransporteDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(UnidadTransporteCreateDTO createDTO);
        Task<UnidadTransporteUpdateDTO> UpdateAsync(int id, UnidadTransporteUpdateDTO updateDTO);
        Task DeleteAsync(int id);
    }
}
