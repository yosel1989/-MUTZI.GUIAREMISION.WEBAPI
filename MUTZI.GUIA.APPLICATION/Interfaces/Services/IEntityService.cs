using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Entity;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IEntityService
    {
        Task<EntityDTO> CreateAsync(EntityCreateDTO newEntity);
        Task<EntityDTO> UpdateAsync(EntityUpdateDTO updateEntity);
        Task<EntityDTO?> GetByIdAsync(int id);
        Task<PagedResult<EntityDTO>> GetDataTableAsync(int pageNumber, int pageSize, string? search, string role);
    }
}
