using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IGuiaRemisionEstadoRepository
    {
        Task<IEnumerable<GuiaRemisionEstado>> GetAllAsync();

        Task<IEnumerable<GuiaRemisionEstado>> GetAllWithPermisosAsync();
    }
}
