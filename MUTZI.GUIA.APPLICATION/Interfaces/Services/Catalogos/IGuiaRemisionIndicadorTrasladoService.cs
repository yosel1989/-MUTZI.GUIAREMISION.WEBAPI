using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IGuiaRemisionIndicadorTrasladoService
    {
        Task<List<GuiaRemisionIndicadorTrasladoToSelectDTO>> GetToSelectAsync();
    }
}
