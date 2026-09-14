using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class GuiaRemisionEntityCarrierRepository: IGuiaRemisionEntityCarrierRepository
    {
        private readonly LogisticaContext _context;
        public GuiaRemisionEntityCarrierRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<GuiaRemisionEntityCarrier> Create(GuiaRemisionEntityCarrier carrier)
        {
            await _context.GuiaRemisionEntityCarriers.AddAsync(carrier);
            await _context.SaveChangesAsync();

            return await GetByGuiaRemisionId(carrier.GuiaRemisionId, strict: true);
        }

        // Private functions

        private async Task<GuiaRemisionEntityCarrier?> GetByGuiaRemisionId(int guiaRemisionId, bool strict)
        {
            var data = await _context.GuiaRemisionEntityCarriers
                .Include(x => x.Entity)
                .Where(x => x.GuiaRemisionId == guiaRemisionId)
                .Select(x => new GuiaRemisionEntityCarrier
                {
                    GuiaRemisionId = x.GuiaRemisionId,
                    EntityId = x.EntityId,
                    Entity = new Entity
                    {
                        DocumentNumber = x.Entity.DocumentNumber,
                        Name = x.Entity.Name,
                        MtcCode = x.Entity.MtcCode,
                        Ubigeo = new Ubigeo
                        {
                            UbigeoId = x.Entity.Ubigeo.UbigeoId,
                            Departamento = x.Entity.Ubigeo.Departamento,
                            Provincia = x.Entity.Ubigeo.Provincia,
                            Distrito = x.Entity.Ubigeo.Distrito
                        }
                    },
                })
                .FirstOrDefaultAsync();

            if(strict && data is null)
            {
                throw new Exception("No se encontro la empresa transportista relacionada a la guía.");
            }

            return data;
        }

    }
}
