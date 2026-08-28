using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IEstablecimientoRepository
    {
        Task<List<Establecimiento>> GetAllByRucToSelectAsync(string ruc);

        Task<List<Establecimiento>> GetAllByRucToSelect2Async(string ruc);
        Task<List<Establecimiento>> GetAllByRucToSelectModal(string ruc);
        Task<List<Establecimiento>> GetListBySuggestion(string ruc, string? search);
        Task<Establecimiento?> GetByIdAsync(int id, bool strict = false, string? tipo = null);
        Task<(List<Establecimiento>, int)> GetListAsync(int pageNumber, int pageSize, string? search);
        Task DeleteAsync(int id);
        Task<Establecimiento> UpdateAsync(Establecimiento establecimiento);
        Task CreateAsync(Establecimiento nuevoEstablecimiento);
        Task<bool> GetBySerieAsync(string ruc, string serie);
        Task<GuiaRemision?> GetLastBySerieAsync(string serie, string ruc);
    }
}
