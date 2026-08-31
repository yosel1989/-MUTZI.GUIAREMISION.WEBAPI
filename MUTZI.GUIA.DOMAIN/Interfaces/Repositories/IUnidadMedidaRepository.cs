using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IUnidadMedidaRepository
    {
        Task<IEnumerable<UnitMeasurement>> GetListPesoToSelectAsync(string? tipo);
    }
}
