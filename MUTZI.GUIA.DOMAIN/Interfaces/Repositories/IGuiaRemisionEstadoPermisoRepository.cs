using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IGuiaRemisionEstadoPermisoRepository
    {
        Task SyncAsync(List<GuiaRemisionEstadoPermiso> permisos);

        Task<IEnumerable<GuiaRemisionEstadoPermiso>> GetByEstado(int guiaRemisionEstadoId);
    }
}
