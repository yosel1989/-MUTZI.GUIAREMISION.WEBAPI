using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IEntidadReguladoraRepository
    {
        Task<IEnumerable<EntidadReguladora>> GetAllAsync();
    }
}
