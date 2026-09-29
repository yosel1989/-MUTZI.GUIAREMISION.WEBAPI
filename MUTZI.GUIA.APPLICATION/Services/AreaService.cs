using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.Area;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class AreaService: IAreaService
    {
        private readonly IAuthService _authService;
        private readonly IAreaRepository _repository;
        private readonly IMapper _mapper;
        public AreaService(
            IAreaRepository repository, 
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<List<AreaToSelectDTO>> GetListToSelectAsync(int? level)
        {
            try
            {
                var data = await _repository.GetListToSelectAsync(level);
                var dataDTO = _mapper.Map<List<AreaToSelectDTO>>( data );

                return dataDTO;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<int> CreateAsync(AreaCreateDTO request)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var newModel = _mapper.Map<Area>( request );

                newModel.CreatedAt = DateTime.Now;
                newModel.CreatedAtUser = currentUser.UserName;
                newModel.CreatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}";
                newModel.CreatedAtEmployeeId = currentUser.EmployeeId;

                var created = await _repository.CreateAsync( newModel );

                return created.AreaId;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<AreaDTO> UpdateAsync(AreaUpdateDTO request)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var currentModel = await _repository.GetByIdAsync( request.AreaId, strict: true );

                currentModel!.UpdatedAt = DateTime.Now;
                currentModel!.UpdatedAtUser = currentUser.UserName;
                currentModel!.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}";
                currentModel!.UpdatedAtEmployeeId = currentUser.EmployeeId;

                var updated = await _repository.UpdateAsync(currentModel);
                var updatedDTO = _mapper.Map<AreaDTO>(updated);

                return updatedDTO;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<AreaDTO?> GetByIdAsync(int areaId, bool strict = false)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var currentModel = await _repository.GetByIdAsync(areaId, strict: true);

                var updatedDTO = _mapper.Map<AreaDTO>(currentModel);

                return updatedDTO;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<PagedResult<AreaDTO>> GetToTableAsync(int pageNumber, int pageSize, string? search)
        {
            var (data, totalRecords) = await _repository.GetToTableAsync(pageNumber, pageSize, search);
            var dataDTO = _mapper.Map<List<AreaDTO>>(data);
            return new PagedResult<AreaDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = dataDTO
            };
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("FOREIGN KEY")) { msg = "No se puede eliminar porque existen registros asociados"; }
                throw new Exception(msg);
            }
        }

        public async Task<ToggleActiveResponseDTO> ToggleActiveAsync(int id, bool active)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var currentModel = await _repository.GetByIdAsync(id, strict: true);

                currentModel!.Active = active;
                currentModel!.UpdatedAt = DateTime.Now;
                currentModel!.UpdatedAtUser = currentUser.UserName;
                currentModel!.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}";
                currentModel!.UpdatedAtEmployeeId = currentUser.EmployeeId;
                var updated = await _repository.UpdateAsync(currentModel);

                return _mapper.Map<ToggleActiveResponseDTO>(currentModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
