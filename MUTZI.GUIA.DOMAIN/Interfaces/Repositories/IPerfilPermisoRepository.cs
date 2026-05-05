using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IPerfilPermisoRepository
    {
        Task SyncAsync(List<PerfilPermiso> perfilPermisos);
    }
}
