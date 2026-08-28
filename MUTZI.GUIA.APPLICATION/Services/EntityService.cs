using AutoMapper;
using Microsoft.Extensions.Logging;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Entity;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class EntityService: IEntityService
    {
        private readonly IEntityRepository _repository;
        private readonly IEntityRoleRepository _entityRoleRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ILogger<EntityService> _logger;
        private readonly IAppDBContext _appDBContext;

        public EntityService(
            IEntityRepository repository,
            IEntityRoleRepository entityRoleRepository,
            IMapper mapper,
            IAuthService authService,
            IAppDBContext appDBContext
        )
        {
            _repository = repository;
            _entityRoleRepository = entityRoleRepository;
            _mapper = mapper;
            _authService = authService;
            _appDBContext = appDBContext;
        }

        
        public async Task<EntityDTO> CreateAsync(EntityCreateDTO newEntity)
        {
            await _appDBContext.BeginTransactionAsync();

            try
            {
                var user = _authService.CurrentUser();
                var existingEntity = await _repository.GetByDocumentNumber(newEntity.DocumentNumber);

                EntityDTO outputEntity = new EntityDTO();

                if (existingEntity is not null && existingEntity.Roles.Any(x => x.Role == newEntity.Role))
                {
                    existingEntity.Type = newEntity.Type;
                    existingEntity.Name = newEntity.Name;
                    existingEntity.FirstName = newEntity.FirstName;
                    existingEntity.LastName = newEntity.LastName;
                    existingEntity.DocumentTypeId = newEntity.DocumentTypeId;
                    existingEntity.DocumentNumber = newEntity.DocumentNumber;
                    existingEntity.UbigeoId = newEntity.UbigeoId;
                    existingEntity.Address = newEntity.Address;
                    existingEntity.CountryId = newEntity.CountryId;
                    existingEntity.IsInternal = newEntity.IsInternal;
                    existingEntity.UpdatedAt = DateTime.UtcNow;
                    existingEntity.UpdatedAtUser = user.UserName;
                    existingEntity.UpdatedAtUserName = $"{user.FirstName} {user.LastName}";
                    var role = await _repository.UpdateAsync(existingEntity);
                    outputEntity = _mapper.Map<EntityDTO>(role);
                }
                else
                {
                    var entity = _mapper.Map<Entity>(newEntity);
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.CreatedAtUser = user.UserName;
                    entity.CreatedAtUserName = $"{user.FirstName} {user.LastName}";
                    var role = await _repository.CreateAsync(existingEntity);
                    outputEntity = _mapper.Map<EntityDTO>(role);
                }

                await _entityRoleRepository.SyncAsync(outputEntity.EntityId, new List<EntityRole> { 
                    new EntityRole { 
                        EntityId = outputEntity.EntityId, 
                        Role = newEntity.Role,
                        CreatedAt = DateTime.UtcNow,
                        CreatedAtUser = user.UserName,
                        CreatedAtUserName = $"{user.FirstName} {user.LastName}"
                    } 
                });

                await _appDBContext.CommitAsync();

                return outputEntity;
            }
            catch (Exception ex)
            {
                await _appDBContext.RollbackAsync();

                throw;
            }

        }

        public async Task<EntityDTO> UpdateAsync(EntityUpdateDTO updateEntity)
        {
            var user = _authService.CurrentUser();
            var entity = await _repository.GetByIdAsync(updateEntity.EntityId);
            if(entity is null) { throw new Exception("Entidad no se encontro"); }

            entity.Type = updateEntity.Type;
            entity.Name = updateEntity.Name;
            entity.FirstName = updateEntity.FirstName;
            entity.LastName = updateEntity.LastName;
            entity.DocumentTypeId = updateEntity.DocumentTypeId;
            entity.DocumentNumber = updateEntity.DocumentNumber;
            entity.UbigeoId = updateEntity.UbigeoId;
            entity.Address = updateEntity.Address;
            entity.CountryId = updateEntity.CountryId;
            entity.IsInternal = updateEntity.IsInternal;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedAtUser = user.UserName;
            entity.UpdatedAtUserName = $"{user.FirstName} {user.LastName}";

            var updated = await _repository.UpdateAsync(entity);
            var updatedDTO = _mapper.Map<EntityDTO>(updated);

            return updatedDTO;
        }

        public async Task<EntityDTO> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is null) { throw new Exception("Entidad no se encontro"); }

            var entityDTO = _mapper.Map<EntityDTO>(entity);
            return entityDTO;
        }

        public async Task<PagedResult<EntityDTO>> GetDataTableAsync(int pageNumber, int pageSize, string? search, string role)
        {
            var ( totalRecords, data ) = await _repository.GetDataTableAsync(pageNumber, pageSize, search, role);
            var dataDTO = _mapper.Map<List<EntityDTO>>(data);
            return new PagedResult<EntityDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = dataDTO
            };
        }

    }
}
