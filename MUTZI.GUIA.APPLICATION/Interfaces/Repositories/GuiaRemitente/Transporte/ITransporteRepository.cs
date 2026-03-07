namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Transporte
{
    public interface ITransporteRepository
    {
        Task<List<DOMAIN.Entities.Transporte>> GetAllAsync();
        Task<(List<DOMAIN.Entities.Transporte>, int)> GetListAsync(int pageNumber, int pageSize);
        Task<DOMAIN.Entities.Transporte?> GetByIdAsync(int id);
        Task CreateAsync(DOMAIN.Entities.Transporte transporte);
        Task UpdateAsync(DOMAIN.Entities.Transporte transporte);
        Task DeleteAsync(int id);
    }
}
