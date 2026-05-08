using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IVentaConfigRepository
    {
        Task<VentaConfig> GetByRucAsync(string ruc);
        Task<List<VentaConfig>> GetOnlyNameAsync();
    }
}
