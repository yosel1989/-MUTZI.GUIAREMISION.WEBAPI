using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories.Catalogo
{
    public class CountryRepository: ICountryRepository
    {
        private readonly LogisticaContext _context;
        public CountryRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Country>> GetListToSelectAsync()
        {
            try
            {
                var data  = await _context.Countries
                    .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }


    }
}
