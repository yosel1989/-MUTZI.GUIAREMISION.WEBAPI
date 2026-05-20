using MUTZI.GUIA.APPLICATION.DTOs.MotivoTraslado;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IMotivoTrasladoService
    {
        Task<List<MotivoTrasladoToSelectDTO>> ListToSelectAsync();
    }
}
