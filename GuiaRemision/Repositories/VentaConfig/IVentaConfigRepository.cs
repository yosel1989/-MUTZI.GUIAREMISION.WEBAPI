using GuiaRemision.Models;
namespace GuiaRemision.Repositories.VentaConfig
{
    public interface IVentaConfigRepository
    {
        Task<Models.DBMS.VentaConfig> GetByRucAsync(string ruc);
    }
}
