using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IAreaRepository
    {
        Task<IEnumerable<Area>> GetListToSelectAsync();
        Task<IEnumerable<Area>> GetListBySuggestion(string? text);
    }
}
