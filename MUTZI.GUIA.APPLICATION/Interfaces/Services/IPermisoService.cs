using MUTZI.GUIA.APPLICATION.DTOs.Permiso;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IPermisoService
    {

        Task<List<PermisoConPerfilesDTO>> ListAsync();
        Task AsignarPerfiles(List<PermisoAsignarPerfilesDTO> permisoPerfiles);
    }
}
