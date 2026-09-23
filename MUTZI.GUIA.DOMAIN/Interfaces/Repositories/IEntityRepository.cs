using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IEntityRepository
    {
        Task<Entity> CreateAsync(Entity newEntity);
        Task<Entity> UpdateAsync(Entity updateEntity);
        Task<Entity?> GetByIdAsync(int id, bool strict = false);
        Task<Entity?> GetByDocumentNumber(string documentNumber);
        Task<(int total, List<Entity>)> GetDataTableAsync(int pageNumber, int pageSize, string? search, string role);
        Task<(int total, List<Entity>)> GetDataTableByRoleAsync(int pageNumber, int pageSize, string? search, string role);
        Task<(int total, List<Entity>)> GetListAsync(int pageNumber, int pageSize, string? search, string? type, string? roles, bool? isInternal, int? excludeId, bool? hasBranch);


        /// Validations 


        /// <summary>
        /// Verifica si una entidad identificada por su número de documento
        /// tiene asignado un rol específico dentro de la base de datos.
        /// </summary>
        /// <param name="documentNumber">
        /// Número de documento único de la entidad (por ejemplo, RUC).
        /// </param>
        /// <param name="role">
        /// Nombre del rol a validar (ejemplo: "PROVEEDOR", "CLIENTE").
        /// </param>
        /// <returns>
        /// Devuelve <c>true</c> si la entidad tiene asignado el rol indicado,
        /// en caso contrario devuelve <c>false</c>.
        /// </returns>
        Task<bool> HasManyRole(string documentNumber, string role);


        /// <summary>
        /// Verifica si una entidad identificada por su número de documento
        /// ya se encuentra dentro de la base de datos.
        /// </summary>
        /// <param name="documentNumber">
        /// Número de documento único de la entidad (por ejemplo, RUC).
        /// </param>
        /// <returns>
        /// Devuelve <c>true</c> si la entidad ya existe,
        /// en caso contrario devuelve <c>false</c>.
        /// </returns>
        Task<bool> HasManyDocumentNumber(string documentNumber);


        Task<List<Entity>> GetByEntityIdS(List<int> entityIds);
    }
}
