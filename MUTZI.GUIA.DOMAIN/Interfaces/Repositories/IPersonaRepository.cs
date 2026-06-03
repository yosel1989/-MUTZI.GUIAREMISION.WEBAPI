using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IPersonaRepository
    {
        Task<IEnumerable<Persona>> GetListBySuggestion(string? text, string? cargo);
        Task<Persona?> GetByIdAsync(int id);
    }
}
