using GuiaRemision.DTOs.Efact;
using GuiaRemision.DTOs.VentaConfig;

namespace GuiaRemision.Services.VentaConfig
{
    public interface IVentaConfigService
    {
        Task<VentaConfigAccessDTO> GetByRucAsync(string ruc);
        Task<TokenResponseDTO> ObtenerTokenAsync(string ruc);
    }
}
