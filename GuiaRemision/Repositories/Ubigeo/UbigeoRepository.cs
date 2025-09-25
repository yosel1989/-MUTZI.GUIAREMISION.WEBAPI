
using GuiaRemision.Models.DBMS;
using Microsoft.EntityFrameworkCore;

namespace GuiaRemision.Repositories.Ubigeo
{
    public class UbigeoRepository : IUbigeoRepository
    {
        private readonly LogisticaContext _context;
        public UbigeoRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<string> GetDistritoAsync(string ubigeo)
        {
            var distrito = await _context.Ubigeos
                .Where(u => u.UbigeoId == ubigeo)
                .Select(u => u.Distrito)
                .FirstOrDefaultAsync();
            if (distrito == null)
            {
                throw new KeyNotFoundException("No encontrado");
            }
            return distrito;
        }
        public Task<string> GetProvinciaAsync(string ubigeo)
        {
            throw new NotImplementedException();
        }
        public Task<string> GetDepartamentoAsync(string ubigeo)
        {
            throw new NotImplementedException();
        }
    }
}
