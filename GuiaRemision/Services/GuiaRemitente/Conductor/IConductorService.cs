using GuiaRemision.DTOs.GuiaRemitente.Conductor;

namespace GuiaRemision.Services.GuiaRemitente.Conductor
{
    public interface IConductorService
    {
        Task<List<ConductorDTO>> GetAllAsync();
        Task<ConductorDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(ConductorCreateDTO createDTO);
        Task<ConductorUpdateDTO> UpdateAsync(int id, ConductorUpdateDTO updateDTO);
        Task DeleteAsync(int id);
    }
}
