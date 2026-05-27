using MUTZI.GUIA.APPLICATION.DTOs.TipoDocumento;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface ITipoDocumentoService
    {
        Task<List<TipoDocumentoDTO>> ListToSelectAsync();
    }
}
