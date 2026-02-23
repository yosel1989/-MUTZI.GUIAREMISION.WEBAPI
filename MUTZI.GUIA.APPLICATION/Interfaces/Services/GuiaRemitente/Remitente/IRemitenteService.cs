using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Remitente
{
    public interface IRemitenteService
    {
        Task<List<RemitenteListDTO>> GetAllAsync();
        Task<RemitenteForGuiaDTO?> GetByIdForGuiaAsync(int id, string tipoGuia);
        Task<RemitenteDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(RemitenteCreateDTO createDTO);
        Task<RemitenteUpdateDTO> UpdateAsync(int id, RemitenteUpdateDTO updateDTO);
        Task DeleteAsync(int id);
    }
}
