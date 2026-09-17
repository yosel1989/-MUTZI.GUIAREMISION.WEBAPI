using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories.Catalogo;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories.Catalogo
{
    public class InvoiceTypeRepository: IInvoiceTypeRepository
    {
        private readonly LogisticaContext _context;
        public InvoiceTypeRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InvoiceType>> GetToSelectAsync()
        {
            var query = _context.InvoiceTypes.AsQueryable();  
            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<InvoiceType?> GetById(int invoiceTypeId, bool strict)
        {
            var data = await _context.InvoiceTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.InvoiceTypeId == invoiceTypeId);

            if(strict && data is null)
            {
                throw new Exception($"Tipo de documento no existe.");
            }

            return data;
        }

    }
}
