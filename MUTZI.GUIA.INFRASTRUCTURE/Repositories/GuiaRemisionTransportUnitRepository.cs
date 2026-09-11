using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class GuiaRemisionTransportUnitRepository: IGuiaRemisionTransportUnitRepository
    {
        private readonly LogisticaContext _context;
        public GuiaRemisionTransportUnitRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<List<GuiaRemisionTransportUnit>> Create(List<GuiaRemisionTransportUnit> relation)
        {
            await _context.GuiaRemisionTransportUnits.AddRangeAsync(relation);
            await _context.SaveChangesAsync();

            return await GetByGuiaRemisionId(relation);
        }

        public async Task<List<GuiaRemisionTransportUnit>> Update(List<GuiaRemisionTransportUnit> relation)
        {
            await _context.SaveChangesAsync();

            return await GetByGuiaRemisionId(relation);
        }

        // Private functions

        private async Task<List<GuiaRemisionTransportUnit>> GetByGuiaRemisionId(List<GuiaRemisionTransportUnit> relation)
        {
            var ids = relation.Select(x => x.TransportUnitId).ToList();
            var data = await _context.GuiaRemisionTransportUnits
                .Include(x => x.TransportUnit)
                .Where(x => ids.Contains(x.TransportUnitId))
                .Select(x => new GuiaRemisionTransportUnit
                {
                    GuiaRemisionId = x.GuiaRemisionId,
                    TransportUnitId = x.TransportUnitId,
                    TransportUnit = new TransportUnit
                    {
                        Placa = x.TransportUnit.Placa,
                    },
                    Type = x.Type
                })
                .ToListAsync();

            return data;
        }

    }
}
