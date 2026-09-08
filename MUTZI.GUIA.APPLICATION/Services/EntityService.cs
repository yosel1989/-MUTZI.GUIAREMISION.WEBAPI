using AutoMapper;
using Microsoft.Extensions.Logging;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Entity;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
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
                EntityDTO outputEntity = new EntityDTO();

                if (newEntity.EntityId == 0)
                {
                    var existingEntity = await _repository.GetByDocumentNumber(newEntity.DocumentNumber);
                    if(existingEntity is null)
                    {
                        var entity = _mapper.Map<Entity>(newEntity);
                        entity.CreatedAt = DateTime.UtcNow;
                        entity.CreatedAtUser = user.UserName;
                        entity.CreatedAtUserName = $"{user.FirstName} {user.LastName}";
                        var entityCreated = await _repository.CreateAsync(entity);
                        outputEntity = _mapper.Map<EntityDTO>(entityCreated);
                    }
                    else
                    {
                        outputEntity = _mapper.Map<EntityDTO>(existingEntity);
                    }
                }
                else
                {
                    var existEntity = await _repository.GetByIdAsync(newEntity.EntityId);
                    outputEntity = _mapper.Map<EntityDTO>(existEntity);
                }

                if(newEntity.Role != "entidad")
                {
                    await _entityRoleRepository.AddAsync( 
                        new EntityRole { 
                            EntityId = outputEntity.EntityId, 
                            Role = newEntity.Role,
                            CreatedAt = DateTime.UtcNow,
                            CreatedAtUser = user.UserName,
                            CreatedAtUserName = $"{user.FirstName} {user.LastName}"
                        } 
                    );
                }
                
                await _appDBContext.CommitAsync();

                return outputEntity;
            }
            catch (Exception ex)
            {
                await _appDBContext.RollbackAsync();

                throw;
            }

        }

        public async Task<EntityDTO> UpdateAsync(int id, EntityUpdateDTO updateEntity)
        {
            var user = _authService.CurrentUser();
            var entity = await _repository.GetByIdAsync(id);
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

        public async Task<PagedResult<EntityByRoleDTO>> GetDataTableByRoleAsync(int pageNumber, int pageSize, string? search, string role)
        {
            var (totalRecords, data) = await _repository.GetDataTableByRoleAsync(pageNumber, pageSize, search, role);
            var dataDTO = _mapper.Map<List<EntityByRoleDTO>>(data);
            return new PagedResult<EntityByRoleDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = dataDTO
            };
        }

        public async Task<PagedResult<EntityListDTO>> GetListAsync(int pageNumber, int pageSize, string? search, string? type, string? roles, bool? isInternal)
        {
            var (totalRecords, data) = await _repository.GetListAsync(pageNumber, pageSize, search, type, roles, isInternal);
            var dataDTO = _mapper.Map<List<EntityListDTO>>(data);
            return new PagedResult<EntityListDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = dataDTO
            };
        }


        public async Task<EstadoActualizadoResponseDTO> ToggleActiveAsync(int id, EstadoActualizadoRequestDTO infoRequestDto)
        {
            try
            {
                var currentUser = _authService.CurrentUser();

                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                {
                    throw new KeyNotFoundException($"Conductor con ID {id} no encontrado.");
                }
                existing.Active = infoRequestDto.Active;
                existing.UpdatedAtUser = currentUser.UserName;
                existing.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                existing.UpdatedAt = DateTime.UtcNow;
                var conductorActualizado = await _repository.UpdateAsync(existing);

                return _mapper.Map<EstadoActualizadoResponseDTO>(conductorActualizado);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
