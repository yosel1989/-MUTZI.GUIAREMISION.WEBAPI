using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Destinatario
{
    public interface IDestinatarioService
    {
        Task<List<DestinatarioDTO>> GetAllAsync();
        Task<DestinatarioDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(DestinatarioCreateDTO createDTO);
        Task<DestinatarioUpdateDTO> UpdateAsync(int id, DestinatarioUpdateDTO updateDTO);
        Task DeleteAsync(int id);
    }
}
