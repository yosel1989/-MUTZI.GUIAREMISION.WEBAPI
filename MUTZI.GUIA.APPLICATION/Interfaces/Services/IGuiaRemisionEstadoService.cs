using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionEstado;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IGuiaRemisionEstadoService
    {
        Task<List<GuiaRemisionEstadoDTO>> GetAllAsync();
        Task<List<GuiaRemisionEstadoWithPermisoDTO>> GetAllWithPermisosAsync();

        Task AsignarPermisosAsync(List<GuiaRemisionEstadoAsignarPermisosDTO> request);
    }
}
