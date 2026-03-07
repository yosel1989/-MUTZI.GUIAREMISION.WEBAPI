namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Destinatario
{
    public interface IDestinatarioRepository
    {
        Task<List<DOMAIN.Entities.Destinatario>> GetAllAsync();
        Task<(List<DOMAIN.Entities.Destinatario>, int)> GetListAsync(int pageNumber, int pageSize);
        Task<DOMAIN.Entities.Destinatario?> GetByIdAsync(int id);
        Task CreateAsync(DOMAIN.Entities.Destinatario destinatario);
        Task UpdateAsync(DOMAIN.Entities.Destinatario destinatario);
        Task DeleteAsync(int id);
    }
}
