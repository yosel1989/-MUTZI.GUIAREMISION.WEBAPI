using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.APPLICATION.DTOs.EntityRole;
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

        public async Task<bool> SyncAsync(int entityId, List<EntityRole> roles)
        {
            var existingRoles = await _context.EntityRoles
                .Where(r => r.EntityId == entityId)
                .ToListAsync();

            var newRoles = roles.Select(r => r.Role).Distinct().ToList();

            var toRemove = existingRoles
                .Where(er => !newRoles.Contains(er.Role))
                .ToList();

            if (toRemove.Any())
                _context.EntityRoles.RemoveRange(toRemove);

            var existingRoleNames = existingRoles.Select(er => er.Role).ToList();
            var toAdd = roles
                .Where(r => !existingRoleNames.Contains(r.Role))
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

        public async Task<bool> AddAsync(EntityRoleCreateDTO entityRole)
        {
            var existingRole = await _context.EntityRoles
                .FirstOrDefaultAsync(r => r.EntityId == entityRole.EntityId && r.Role == entityRole.Role);

            if (existingRole != null)
            {

            }

            return true;
        }

    }
}
