using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetListToSelectAsync();
    }
}
