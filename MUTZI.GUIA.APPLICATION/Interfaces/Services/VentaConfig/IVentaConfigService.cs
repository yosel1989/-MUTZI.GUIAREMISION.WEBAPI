using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.VentaConfig;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.VentaConfig
{
    public interface IVentaConfigService
    {
        Task<VentaConfigAccessDTO> GetByRucAsync(string ruc);
        Task<TokenResponseDTO> ObtenerTokenAsync(string ruc);
    }
}
