using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IEstablecimientoRepository
    {
        Task<List<Establecimiento>> GetAllByRucToSelectAsync();
    }
}
