using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IUnidadMedidaRepository
    {
        Task<List<UnidadMedida>> GetListToSelectAsync();
        Task<List<UnidadMedida>> GetListPesoToSelectAsync();
    }
}
