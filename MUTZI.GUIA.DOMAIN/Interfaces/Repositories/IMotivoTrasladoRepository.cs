using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IMotivoTrasladoRepository
    {
        Task<List<MotivoTraslado>> GetListToSelectAsync();
        Task<MotivoTraslado?> GetByIdAsync(int motivoTrasladoId);
    }
}
