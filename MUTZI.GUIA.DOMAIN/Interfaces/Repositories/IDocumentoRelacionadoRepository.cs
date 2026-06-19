using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IDocumentoRelacionadoRepository
    {
        Task<IEnumerable<DocumentoRelacionado>> GetAllAsync();
        Task<IEnumerable<DocumentoRelacionado>> GetAllToSelectAsync(string? tipo);
    }
}
