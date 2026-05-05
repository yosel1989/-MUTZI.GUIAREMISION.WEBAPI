using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IPermisoRepository
    {

        Task<List<Permiso>> GetListAsync();
    }
}
