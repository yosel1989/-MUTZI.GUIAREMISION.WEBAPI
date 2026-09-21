using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class EntityBranchRepository: IEntityBranchRepository
    {
        private readonly LogisticaContext _context;
        public EntityBranchRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<List<EntityBranch>> GetAllByEntityToSelectAsync(int entityId)
        {
            try
            {
                var data = await _context.Establecimiento
                    .Where(e => e.EntityId == entityId && e.Active)
                    .Include(r=>r.Ubigeo)
                    .OrderBy(e => e.Description)
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }

        public async Task<List<EntityBranch>> GetAllByEntityToSelect2Async(int entityId)
        {
            try
            {
                var data = await _context.Establecimiento
                    .Include(r => r.Ubigeo)
                    .Where(e => e.EntityId == entityId && e.Active)
                    .Select(m => new EntityBranch
                    {
                        EntityBranchId = m.EntityBranchId,
                        Description = m.Description,
                    })
                    .OrderBy(e => e.Description)
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<EntityBranch>> GetAllByEntityToSelectModal(int entityId)
        {
            try
            {
                var data = await _context.Establecimiento
                .Select(r => new EntityBranch
                {
                    EntityBranchId = r.EntityBranchId,
                    Description = r.Description,
                    CodeSunat = r.CodeSunat,
                })
                .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<EntityBranch>> GetListBySuggestion(int entityId, string? search)
        {
            try
            {
                var query = _context.Establecimiento
                .Include(r => r.Entity)
                .AsQueryable();

                if (!String.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c =>
                        (string.IsNullOrEmpty(search) ||
                         c.CodeSunat.Contains(search) ||
                         c.Description.Contains(search)
                        )
                    );
                }

                var data = await query
                .Where(q => q.EntityId == entityId)
                .Where(q => q.Active)
                .Select(m => new EntityBranch
                {
                    EntityBranchId = m.EntityBranchId,
                    Description = m.Description,
                    CodeSunat = m.CodeSunat,
                    Entity = new Entity
                    {
                        Name = m.Entity.Name,
                        DocumentNumber = m.Entity.DocumentNumber,
                        DocumentType = m.Entity.DocumentType,
                    }
                })
                .AsNoTracking()
                .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<EntityBranch?> GetByIdAsync(int id, bool strict, string? tipo)
        {
            try
            {
                var data = await _context.Establecimiento
                    .Include(r => r.Ubigeo)
                    .Include(r => r.Series)
                    .Include(r => r.Entity)
                        .ThenInclude(r => r.Ubigeo)
                    //.AsNoTracking()
                    .FirstOrDefaultAsync(r => r.EntityBranchId == id);

                if(strict && data is null)
                {
                    throw new Exception($"No existe el establecimiento ${ (tipo is null ? "" : "del " + tipo) } en los registros.");
                }

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<(List<EntityBranch>, int)> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            try
            {
                var query = _context.Establecimiento
                //.Include(r => r.Ubigeo)
                //.Include(e => e.Entity)
                .AsQueryable();
                if (!string.IsNullOrEmpty(search))
                {
                    var val = search.ToLower();
                    query = query.Where(x =>
                        x.Entity.DocumentNumber.Contains(val) ||
                        x.Entity.Name.ToLower().Contains(val) ||
                        x.Entity.FirstName.ToLower().Contains(val) ||
                        x.Entity.LastName.ToLower().Contains(val) ||
                        x.Description.ToLower().Contains(val) ||
                        x.Ubigeo.Departamento.ToLower().Contains(val) ||
                        x.Ubigeo.Provincia.ToLower().Contains(val) ||
                        x.Ubigeo.Distrito.ToLower().Contains(val) ||
                        x.Address.ToLower().Contains(val) ||
                        x.Email.ToLower().Contains(val) ||
                        x.CodeSunat.ToLower().Contains(val) ||
                        x.CreatedAtUserName.ToLower().Contains(val) ||
                        x.UpdatedAtUserName.ToLower().Contains(val)
                    );
                }
                var totalRecords = await query.CountAsync();

                var data = await query
                    .OrderByDescending(r => r.EntityBranchId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new EntityBranch
                    {
                        EntityBranchId = x.EntityBranchId,
                        EntityId = x.EntityId,
                        Entity = new Entity
                        {
                            Name = x.Entity.Name,
                            FirstName = x.Entity.FirstName,
                            LastName = x.Entity.LastName,
                            Address = x.Entity.Address,
                            DocumentNumber = x.Entity.DocumentNumber,
                            DocumentType = new TipoDocumento
                            {
                                Descripcion = x.Entity.DocumentType.Descripcion,
                                DescripcionCorta = x.Entity.DocumentType.DescripcionCorta
                            }
                        },
                        Description = x.Description,
                        Address = x.Address,
                        CodeSunat = x.CodeSunat,
                        UbigeoId = x.UbigeoId,
                        Ubigeo = new Ubigeo
                        {
                            Departamento = x.Ubigeo.Departamento,
                            Provincia = x.Ubigeo.Provincia,
                            Distrito = x.Ubigeo.Distrito,
                        },
                        Active = x.Active,
                        SeriesCount = x.Series.Count(x => x.Active),
                        CreatedAt = x.CreatedAt,
                        CreatedAtUser = x.CreatedAtUser,
                        CreatedAtUserName = x.CreatedAtUserName,
                        UpdatedAt = x.UpdatedAt,
                        UpdatedAtUser = x.UpdatedAtUser,
                        UpdatedAtUserName = x.UpdatedAtUserName
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return (data, totalRecords);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var model = await _context.Establecimiento.FindAsync(id);
                if (model != null)
                {
                    _context.Establecimiento.Remove(model);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<EntityBranch> UpdateAsync(EntityBranch actualizarEstablecimiento)
        {
            try
            {
                actualizarEstablecimiento.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                //await _context.Entry(actualizarEstablecimiento).ReloadAsync();
                return await GetByIdAsync(actualizarEstablecimiento.EntityBranchId, true, null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateAsync(EntityBranch nuevoEstablecimiento)
        {
            try
            {
                await _context.Establecimiento.AddAsync(nuevoEstablecimiento);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> GetByCodeSunatAsync(int entityId, string codeSunat)
        {
            try
            {
                var existe = await _context.Establecimiento
                .AnyAsync(r => r.EntityId == entityId && r.CodeSunat == codeSunat);
                return existe;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<GuiaRemision?> GetLastBySerieAsync(string serie, int entityId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(serie))
                    return null;

                var data = await _context.GuiaRemision
                    .FirstOrDefaultAsync() ?? null;

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}
