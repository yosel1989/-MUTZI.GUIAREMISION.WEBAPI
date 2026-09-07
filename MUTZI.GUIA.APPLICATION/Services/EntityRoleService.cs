using AutoMapper;
using Microsoft.Extensions.Logging;
using MUTZI.GUIA.APPLICATION.DTOs.EntityRole;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class EntityRoleService: IEntityRoleService
    {
        private readonly IEntityRoleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ILogger<EntityRoleService> _logger;
        private readonly IAppDBContext _appDBContext;

        public EntityRoleService(
            IEntityRoleRepository repository,
            IMapper mapper,
            IAuthService authService,
            IAppDBContext appDBContext
        )
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
            _appDBContext = appDBContext;
        }


        public List<EntityRoleToSelectDTO> GetListToSelect()
        {
            return new List<EntityRoleToSelectDTO> { 
                new EntityRoleToSelectDTO{Label = "Cliente", Role = "cliente" },
                new EntityRoleToSelectDTO{Label = "Proveedor", Role = "proveedor" },
                new EntityRoleToSelectDTO{Label = "Transportista", Role = "transportista" },
                new EntityRoleToSelectDTO{Label = "Emisor", Role = "emisor" }
            };
        }

        public async Task<List<EntityRoleDTO>> GetByEntityAsync(int entityId)
        {
            var data = await _repository.GetByEntityAsync(entityId);
            var dataDTO = _mapper.Map<List<EntityRoleDTO>>(data);

            return dataDTO;
        }

        public async Task<bool> SyncToEntityAsync(EntityRoleToSyncToEntityDTO request)
        {
            var currentUser = _authService.CurrentUser();

            await _repository.SyncToEntityAsync(request.EntityId, request.Roles.Select(r => new EntityRole
            {
                EntityId = request.EntityId,
                Role = r,
                CreatedAt = DateTime.UtcNow,
                CreatedAtUser = currentUser.UserName,
                CreatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}",
                UpdatedAt = DateTime.UtcNow,
                UpdatedAtUser = currentUser.UserName,
                UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}"
            }).ToList(), currentUser.UserName, $"{currentUser.FirstName} {currentUser.LastName}");

            return true;
        }
    }
}
