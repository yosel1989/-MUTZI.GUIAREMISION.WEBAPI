using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface ITipoDocumentoRepository
    {
        Task<IEnumerable<TipoDocumento>> GetListToSelectAsync();
    }
}
