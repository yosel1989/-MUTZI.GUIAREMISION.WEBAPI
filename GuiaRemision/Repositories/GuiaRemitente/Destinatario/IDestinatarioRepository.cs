namespace GuiaRemision.Repositories.GuiaRemitente.Destinatario
{
    public interface IDestinatarioRepository
    {
        Task<List<Models.DBMS.Destinatario>> GetAllAsync();
        Task<Models.DBMS.Destinatario?> GetByIdAsync(int id);
        Task CreateAsync(Models.DBMS.Destinatario destinatario);
        Task UpdateAsync(Models.DBMS.Destinatario destinatario);
        Task DeleteAsync(int id);
    }
}
