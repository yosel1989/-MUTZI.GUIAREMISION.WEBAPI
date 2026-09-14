using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Entity;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IEntityService
    {
        Task<EntityDTO> CreateAsync(EntityCreateDTO newEntity);

        Task<EntityDTO> UpdateAsync(int id, EntityUpdateDTO updateEntity);

        Task<EntityDTO?> GetByIdAsync(int id);

        Task<PagedResult<EntityDTO>> GetDataTableAsync(int pageNumber, int pageSize, string? search, string role);

        Task<PagedResult<EntityByRoleDTO>> GetDataTableByRoleAsync(int pageNumber, int pageSize, string? search, string role);

        Task<PagedResult<EntityListDTO>> GetListAsync(int pageNumber, int pageSize, string? search, string? type, string? roles, bool? isInternal, int? excludeId);

        Task<EstadoActualizadoResponseDTO> ToggleActiveAsync(int id, EstadoActualizadoRequestDTO infoRequestDto);

    }
}
