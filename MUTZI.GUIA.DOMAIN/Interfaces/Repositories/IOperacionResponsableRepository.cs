using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IOperacionResponsableRepository
    {
        Task<IEnumerable<OperacionResponsable>> GetListToSelectAsync();
        Task<OperacionResponsable> CreateAsync(OperacionResponsable newModel);
        Task<OperacionResponsable?> GetByIdAsync(int id);
        Task<(List<OperacionResponsable>, int)> GetListAsync(int pageNumber, int pageSize, string? search);
        Task<OperacionResponsable> UpdateAsync(OperacionResponsable actualizarModel);

    }
}
