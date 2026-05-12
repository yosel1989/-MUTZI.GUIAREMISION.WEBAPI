using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IGuiaRemisionHistorialRepository
    {
        Task<List<GuiaRemisionHistorial>> ObtenerListadoByGuiaAsync(int guiaId);
        Task<GuiaRemisionHistorial> RegistrarAsync(GuiaRemisionHistorial guiaRemisionHistorial);
    }
}
