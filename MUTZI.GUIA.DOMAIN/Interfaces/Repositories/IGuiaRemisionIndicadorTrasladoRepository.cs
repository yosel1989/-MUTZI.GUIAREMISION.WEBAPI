using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IGuiaRemisionIndicadorTrasladoRepository
    {
        Task<IEnumerable<GuiaRemisionIndicadorTraslado>> GetListToSelectAsync();
    }
}
