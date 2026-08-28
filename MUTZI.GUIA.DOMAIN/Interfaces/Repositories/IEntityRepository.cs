using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IEntityRepository
    {
        Task<Entity> CreateAsync(Entity newEntity);
        Task<Entity> UpdateAsync(Entity updateEntity);
        Task<Entity?> GetByIdAsync(int id);
        Task<Entity?> GetByDocumentNumber(string documentNumber);
        Task<(int total, List<Entity>)> GetDataTableAsync(int pageNumber, int pageSize, string? search, string role);
    }
}
