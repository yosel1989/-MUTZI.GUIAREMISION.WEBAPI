using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IBienNormalizadoRepository
    {
        Task<IEnumerable<BienNormalizado>> GetAllAsync();
        Task<IEnumerable<BienNormalizado>> GetAllToSelectAsync();
    }
}
