using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class AreaRepository: IAreaRepository
    {
        private readonly LogisticaContext _context;
        public AreaRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Area>> GetListToSelectAsync(int? level)
        {
            try
            {
                var query = _context.Area
                            .AsQueryable();

                if (level != null)
                {
                    query = query.Where(x => x.Level == level);
                }



                var data = await query.Select(x => new Area
                {
                    AreaId = x.AreaId,
                    Code = x.Code,
                    Name = x.Name,
                })
                    .AsNoTracking()
                    .ToListAsync();

                return data;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<Area> CreateAsync(Area newModel)
        {
            try
            {
                await _context.Area.AddAsync(newModel);
                await _context.SaveChangesAsync();

                return newModel;
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.Contains("key 'area.uk_area_code'"))
                {
                    throw new Exception("El código de área ya se encuentra registrado");
                }
                throw;
            }
        }
        public async Task<Area> UpdateAsync(Area newModel)
        {
            try
            {
                await _context.SaveChangesAsync();

                return newModel;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<Area?> GetByIdAsync(int areaId, bool strict = false)
        {
            try
            {
                var model = await _context.Area.Where(x => x.AreaId == areaId)
                    .FirstOrDefaultAsync();

                if(model is null && strict)
                {
                    throw new Exception("No se encontró el area registrada.");
                }

                return model;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<(List<Area>, int)> GetToTableAsync(int pageNumber, int pageSize, string? search)
        {
            try
            {
                var query = _context.Area
                    .AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    var val = search.ToLower().Trim();
                    query = query.Where(x =>
                        x.Name.ToLower().Contains(val)
                    );
                }

                var totalRecords = await query.CountAsync();
                var data = await query
                    .OrderByDescending(x => x.CreatedAt)
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

        public async Task DeleteAsync(int id)
        {
            try
            {
                var model = await _context.Area.FindAsync(id);
                if (model != null)
                {
                    _context.Area.Remove(model);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
