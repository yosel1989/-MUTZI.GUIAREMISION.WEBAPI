
using GuiaRemision.DTOs.GuiaRemitente;
using GuiaRemision.Models.DBMS;
using Microsoft.EntityFrameworkCore;

namespace GuiaRemision.Repositories.VentaConfig
{
    public class VentaConfigRepository : IVentaConfigRepository
    {
        private readonly LogisticaContext _context;
        public VentaConfigRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<Models.DBMS.VentaConfig> GetByRucAsync(string ruc)
        {
            return await _context.VentaConfigs
                .FirstOrDefaultAsync(vc => vc.Ruc == ruc)
                ?? throw new KeyNotFoundException("No encontrado"); ;
        }
    }
}
