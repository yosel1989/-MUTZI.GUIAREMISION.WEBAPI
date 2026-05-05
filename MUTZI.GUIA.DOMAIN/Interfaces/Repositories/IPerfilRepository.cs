using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IPerfilRepository
    {
        Task<Perfil?> GetByIdAsync(int id);
        Task<(List<Perfil>, int)> GetListAsync(int pageNumber, int pageSize, string? search);
        Task DeleteAsync(int id);
        Task<Perfil> UpdateAsync(Perfil actualizarPerfil);
        Task CreateAsync(Perfil nuevoPerfil);
    }
}
