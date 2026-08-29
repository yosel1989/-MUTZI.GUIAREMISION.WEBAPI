using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IEntityRoleRepository
    {
        Task<bool> SyncAsync(int entityId, List<EntityRole> roles);
        Task<EntityRole?> GetByIdAsync(int id);
        Task<IEnumerable<EntityRole>> GetByEntityAsync(int entityId);
        Task<bool> AddAsync(EntityRole entityRole);
    }
}
