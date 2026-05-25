using MUTZI.GUIA.APPLICATION.DTOs.UnidadMedida;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IUnidadMedidaService
    {
        Task<List<UnidadMedidaToSelectDTO>> ListToSelectAsync();
        Task<List<UnidadMedidaToSelectDTO>> ListPesoToSelectAsync();
    }
}
