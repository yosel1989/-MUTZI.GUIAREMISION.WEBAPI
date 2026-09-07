using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class EntityRoleRepository : IEntityRoleRepository
    {
        private readonly LogisticaContext _context;
        public EntityRoleRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<bool> SyncToEntityAsync(int entityId, List<EntityRole> roles, string user, string userName)
        {
            var existingRoles = await _context.EntityRoles
                .Where(r => r.EntityId == entityId)
                .ToListAsync();

            var newRoles = roles.Select(r => r.Role).Distinct().ToList();

            var toDeactivate = existingRoles
                .Where(er => !newRoles.Contains(er.Role) && er.Active)
                .ToList();

            foreach (var role in toDeactivate)
            {
                var r = roles.Find(r => r.Role == role.Role);
                role.Active = false;
                role.UpdatedAt = DateTime.UtcNow;
                role.UpdatedAtUser = user;
                role.UpdatedAtUserName = userName;
            }

            var toReactivate = existingRoles
                .Where(er => newRoles.Contains(er.Role) && !er.Active)
                .ToList();

            foreach (var role in toReactivate)
            {
                var r = roles.Find(r => r.Role == role.Role);
                role.Active = true;
                role.UpdatedAt = DateTime.UtcNow;
                role.UpdatedAtUser = user;
                role.UpdatedAtUserName = userName;
            }

            var existingRoleNames = existingRoles.Select(er => er.Role).ToList();
            var toAdd = roles
                .Where(r => !existingRoleNames.Contains(r.Role))
                .Select(r => new EntityRole
                {
                    EntityId = entityId,
                    Role = r.Role,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedAtUser = user,
                    CreatedAtUserName = userName
                })
                .ToList();

            if (toAdd.Any())
                await _context.EntityRoles.AddRangeAsync(toAdd);

            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<EntityRole?> GetByIdAsync(int id)
        {
            var role = await _context.EntityRoles
                .FindAsync(id);
            return role;
        }

        public async Task<IEnumerable<EntityRole>> GetByEntityAsync(int entityId)
        {
            var roles = await _context.EntityRoles
                .Where(r => r.EntityId == entityId)
                .AsNoTracking()
                .ToListAsync();
            return roles;
        }

        public async Task<bool> AddAsync(EntityRole newEntityRole)
        {
            var existingRole = await _context.EntityRoles
                .FirstOrDefaultAsync(r => r.EntityId == newEntityRole.EntityId && r.Role == newEntityRole.Role);

            if (existingRole == null)
            {
                await _context.EntityRoles.AddAsync(newEntityRole);
                await _context.SaveChangesAsync();
            }

            return true;
        }

    }
}
