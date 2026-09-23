using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class EntityBranchSerieRepository: IEntityBranchSerieRepository
    {
        private readonly LogisticaContext _context;
        public EntityBranchSerieRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<EntityBranchSerie?> CreateAsync(EntityBranchSerie newEntityBranchSerie)
        {
            try
            {
                await _context.EntityBranchSeries.AddAsync(newEntityBranchSerie);
                await _context.SaveChangesAsync();

                var created = await GetByIdAsync(newEntityBranchSerie.EntityBranchSerieId, strict: true);
                return created;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<EntityBranchSerie?> UpdateAsync(EntityBranchSerie editEntityBranchSerie)
        {
            try
            {
                await _context.SaveChangesAsync();
                var updated = await GetByIdAsync(editEntityBranchSerie.EntityBranchSerieId, strict: true);
                return updated;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<EntityBranchSerie?> GetByIdAsync(int entityBranchSerieId, bool strict)
        {
            try
            {
                var data =await _context.EntityBranchSeries
                    .Include(x => x.InvoiceType)
                    .Where(x => x.EntityBranchSerieId == entityBranchSerieId)
                    .FirstOrDefaultAsync();

                if(strict && data is null)
                {
                    throw new Exception($"No existe la serie del establecimiento en los registros.");
                }

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<EntityBranchSerie>, int)> GetListAsync(int entityBranchId, int pageNumber, int pageSize, string? search)
        {
            try
            {
                var query = _context.EntityBranchSeries
                .Include(r => r.InvoiceType)
                .Where(x => x.EntityBranchId == entityBranchId)
                .AsQueryable();
                if (!string.IsNullOrEmpty(search))
                {
                    var val = search.ToLower();
                    query = query.Where(x =>
                        x.InvoiceType.Name.Contains(val) ||
                        x.CreatedAtUserName.ToLower().Contains(val) ||
                        x.Area.ToLower().Contains(val) ||
                        x.UpdatedAtUserName.ToLower().Contains(val)
                    );
                }
                var totalRecords = await query.CountAsync();

                var data = await query
                    .OrderByDescending(r => r.EntityBranchId)
                    .Skip((pageNumber - 1) * pageSize)
                    .AsNoTracking()
                    .Take(pageSize)
                    .ToListAsync();

                return (data, totalRecords);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<EntityBranchSerie>> GetAllByEntityToSelectModal(int entityId)
        {
            try
            {
                var data = await _context.EntityBranchSeries
                    .Where(x =>
                        x.EntityBranch.Active == true &&
                        x.EntityBranch.EntityId == entityId &&
                        x.Active == true
                    )
                .Select(r => new EntityBranchSerie
                {
                    EntityBranchSerieId = r.EntityBranchSerieId,
                    Serie = r.Serie,
                    EntityBranch = new EntityBranch { 
                        Entity = new Entity { 
                            Name = r.EntityBranch.Entity.Name,
                            DocumentNumber = r.EntityBranch.Entity.DocumentNumber
                        }
                    }
                })
                .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Helpers

        public async Task<bool> HasSerie(int entityBranchId, int? entityBranchSerieId, string serie)
        {
            var query = _context.EntityBranchSeries.AsQueryable();

            if (entityBranchSerieId.HasValue)
            {
                return await query.AnyAsync(x =>
                    x.EntityBranchId == entityBranchId
                    && x.EntityBranchSerieId != entityBranchSerieId
                    && x.Serie == serie);
            }
            else
            {
                return await query.AnyAsync(x =>
                    x.EntityBranchId == entityBranchId
                    && x.Serie == serie);
            }
        }


    }
}
