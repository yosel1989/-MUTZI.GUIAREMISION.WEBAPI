using GuiaRemision.DTOs.GuiaRemitente.Remitente;

namespace GuiaRemision.Services.GuiaRemitente.Remitente
{
    public interface IRemitenteService
    {
        Task<List<RemitenteDTO>> GetAllAsync();
        Task<RemitenteDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(RemitenteCreateDTO createDTO);
        Task<RemitenteUpdateDTO> UpdateAsync(int id, RemitenteUpdateDTO updateDTO);
        Task DeleteAsync(int id);
    }
}
