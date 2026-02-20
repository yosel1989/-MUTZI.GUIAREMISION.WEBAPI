namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Remitente
{
    public interface IRemitenteRepository
    {
        Task<List<DOMAIN.Entities.Remitente>> GetAllAsync();
        Task<DOMAIN.Entities.Remitente?> GetByIdAsync(int id, string tipoGuia);
        Task CreateAsync(DOMAIN.Entities.Remitente remitente);
        Task UpdateAsync(DOMAIN.Entities.Remitente remitente);
        Task DeleteAsync(int id);
        Task<DOMAIN.Entities.GuiaRemision?> GetLastBySerieAsync(string serie);
    }
}
