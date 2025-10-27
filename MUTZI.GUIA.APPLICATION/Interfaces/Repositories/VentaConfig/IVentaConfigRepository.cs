namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.VentaConfig
{
    public interface IVentaConfigRepository
    {
        Task<DOMAIN.Entities.VentaConfig> GetByRucAsync(string ruc);
    }
}
