using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.EntityBranchSerie;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class EntityBranchSerieService: IEntityBranchSerieService
    {
        private readonly IEntityBranchSerieRepository _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public EntityBranchSerieService(
            IEntityBranchSerieRepository repository,
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }


        public async Task<int> CreateAsync(EntityBranchSerieCreateDTO request)
        {
            var currentUser = _authService.CurrentUser();
            var existe = await _repository.HasSerie(request.EntityBranchId, null, request.Serie);
            if (existe) { throw new Exception("La serie ya se encuentra registrada en el establecimiento"); }
            try
            {
                var newEntityBranchSerie = _mapper.Map<EntityBranchSerie>(request);
                newEntityBranchSerie.CreatedAtUser = currentUser.UserName;
                newEntityBranchSerie.CreatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                newEntityBranchSerie.CreatedAt = DateTime.UtcNow;
                await _repository.CreateAsync(newEntityBranchSerie);
                return newEntityBranchSerie.EntityBranchSerieId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<EntityBranchSerieDTO> UpdateAsync(int entityBranchSerieId, EntityBranchSerieUpdateDTO request)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var entityBranchSerie = await _repository.GetByIdAsync(entityBranchSerieId, strict: true);
                var existe = await _repository.HasSerie(request.EntityBranchId, request.EntityBranchSerieId, request.Serie);
                if (existe) { throw new Exception("La serie ya se encuentra registrada en el establecimiento"); }

                entityBranchSerie!.Serie = request.Serie;
                entityBranchSerie!.Area = request.Area;
                entityBranchSerie!.UpdatedAt = DateTime.UtcNow;
                entityBranchSerie!.UpdatedAtUser = currentUser.UserName;
                entityBranchSerie!.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                var establecimientoActualizado = await _repository.UpdateAsync(entityBranchSerie);
                return _mapper.Map<EntityBranchSerieDTO>(establecimientoActualizado);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<EntityBranchSerieDTO?> GetByIdAsync(int entityBranchSerieId)
        {
            var data = await _repository.GetByIdAsync(entityBranchSerieId, strict: true);
            return _mapper.Map<EntityBranchSerieDTO>(data);
        }


        public async Task<PagedResult<EntityBranchSerieDTO>> GetListAsync(int entityBranchId, int pageNumber, int pageSize, string? search)
        {
            var (data, totalRecords) = await _repository.GetListAsync(entityBranchId, pageNumber, pageSize, search);

            return new PagedResult<EntityBranchSerieDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = _mapper.Map<List<EntityBranchSerieDTO>>(data)
            };
        }

        public async Task<ToggleActiveResponseDTO> ToggleActiveAsync(int entityBranchSerieId, bool active)
        {
            var currentUser = _authService.CurrentUser();
            var existing = await _repository.GetByIdAsync(entityBranchSerieId);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Serie con ID {entityBranchSerieId} no encontrado.");
            }
            existing.Active = active;
            existing.UpdatedAtUser = currentUser.UserName;
            existing.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
            existing.UpdatedAt = DateTime.UtcNow;
            var updated = await _repository.UpdateAsync(existing);

            return _mapper.Map<ToggleActiveResponseDTO>(updated);
        }

        public async Task<List<EntityBranchSerieToSelectDTO>> GetAllByEntityToSelectModal(int entityId, string? search)
        {
            try
            {
                var data = await _repository.GetAllByEntityToSelectModal(entityId, search);
                var dataDTO = _mapper.Map<List<EntityBranchSerieToSelectDTO>>(data);
                return dataDTO;
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
