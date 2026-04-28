using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IEstablecimientoRepository
    {
        Task<List<Establecimiento>> GetAllByRucToSelectAsync(string ruc);
        Task<List<Establecimiento>> GetAllByRucToSelectModal(string ruc);
        Task<List<Establecimiento>> GetListBySuggestion(string ruc, string? search);
        Task<Establecimiento?> GetByIdAsync(int id);
    }
}
