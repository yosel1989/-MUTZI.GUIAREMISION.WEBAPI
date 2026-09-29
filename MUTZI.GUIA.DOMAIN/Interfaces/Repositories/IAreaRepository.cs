using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IAreaRepository
    {
        Task<IEnumerable<Area>> GetListToSelectAsync(int? level);
        Task<Area> CreateAsync(Area newModel);
        Task<Area> UpdateAsync(Area newModel);
        Task<Area?> GetByIdAsync(int areaId, bool strict = false);
        Task<(List<Area>, int)> GetToTableAsync(int pageNumber, int pageSize, string? search);
        Task DeleteAsync(int id);
    }
}
