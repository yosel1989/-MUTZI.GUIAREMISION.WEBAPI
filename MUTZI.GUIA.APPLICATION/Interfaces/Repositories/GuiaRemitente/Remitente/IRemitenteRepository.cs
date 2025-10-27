namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Remitente
{
    public interface IRemitenteRepository
    {
        Task<List<DOMAIN.Entities.Remitente>> GetAllAsync();
        Task<DOMAIN.Entities.Remitente?> GetByIdAsync(int id);
        Task CreateAsync(DOMAIN.Entities.Remitente remitente);
        Task UpdateAsync(DOMAIN.Entities.Remitente remitente);
        Task DeleteAsync(int id);
    }
}
