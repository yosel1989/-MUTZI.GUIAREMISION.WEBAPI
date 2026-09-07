using MUTZI.GUIA.APPLICATION.DTOs.EntityRole;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IEntityRoleService
    {
        List<EntityRoleToSelectDTO> GetListToSelect();

        Task<List<EntityRoleDTO>> GetByEntityAsync(int entityId);

        Task<bool> SyncToEntityAsync(EntityRoleToSyncToEntityDTO request);
    }
}
