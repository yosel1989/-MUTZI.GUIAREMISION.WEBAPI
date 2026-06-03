using MUTZI.GUIA.APPLICATION.DTOs.Persona;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IPersonaService
    {
        Task<IEnumerable<PersonaSugeridaDTO>> GetListBySuggestion(string? text);
        Task<PersonaDTO> GetByIdAsync(int id);
    }
}
