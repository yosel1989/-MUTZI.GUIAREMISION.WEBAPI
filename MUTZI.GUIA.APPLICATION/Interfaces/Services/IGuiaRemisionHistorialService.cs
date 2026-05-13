using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionHistorial;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IGuiaRemisionHistorialService
    {
        Task<IEnumerable<GuiaRemisionHistorialListDTO>> GetAllByGuiaAsync(int guiaRemisionId);
    }
}
