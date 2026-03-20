
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.VentaConfig;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class VentaConfigRepository : IVentaConfigRepository
    {
        private readonly LogisticaContext _context;
        public VentaConfigRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<DOMAIN.Entities.VentaConfig> GetByRucAsync(string ruc)
        {
            return await _context.VentaConfigs
                .FirstOrDefaultAsync(vc => vc.Ruc == ruc);
        }
        public async Task<List<VentaConfig>> GetOnlyNameAsync()
        {
            return await _context.VentaConfigs
            .Select(e => new VentaConfig
            {
                Ruc = e.Ruc,
                Empresa = e.Empresa
            })
            .ToListAsync();
        }
    }
}
