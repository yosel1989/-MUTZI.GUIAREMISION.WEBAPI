using AutoMapper;
using Microsoft.Extensions.Logging;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.IntegrationCredential;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class IntegrationCredentialService: IIntegrationCredentialService
    {
        private readonly IIntegrationCredentialRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ILogger<IntegrationCredentialService> _logger;
        private readonly IAppDBContext _appDBContext;

        public IntegrationCredentialService(
            IIntegrationCredentialRepository repository,
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

        
        public async Task<IntegrationCredentialDTO> CreateAsync(IntegrationCredentialCreateDTO newIntegrationCredential)
        {

            try
            {
                var user = _authService.CurrentUser();
                var inputIntegrationCredential = _mapper.Map<IntegrationCredential>(newIntegrationCredential);
                inputIntegrationCredential.CreatedAt = DateTime.UtcNow;
                inputIntegrationCredential.CreatedAtUser = user.UserName;
                inputIntegrationCredential.CreatedAtUserName = $"{user.FirstName} {user.LastName}";

                var created = await _repository.CreateAsync(inputIntegrationCredential);
                var createdDTO = _mapper.Map<IntegrationCredentialDTO>(created);

                return createdDTO;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<IntegrationCredentialDTO> UpdateAsync(int id, IntegrationCredentialUpdateDTO updateIntegrationCredential)
        {
            var user = _authService.CurrentUser();
            var IntegrationCredential = await _repository.GetByIdAsync(id);
            if(IntegrationCredential is null) { throw new Exception("No se encontro la credencial."); }

            IntegrationCredential.Provider = updateIntegrationCredential.Provider;
            IntegrationCredential.Key = updateIntegrationCredential.Key;
            IntegrationCredential.KeyTest = updateIntegrationCredential.KeyTest;
            IntegrationCredential.UpdatedAt = DateTime.UtcNow;
            IntegrationCredential.UpdatedAtUser = user.UserName;
            IntegrationCredential.UpdatedAtUserName = $"{user.FirstName} {user.LastName}";

            var updated = await _repository.UpdateAsync(IntegrationCredential);
            var updatedDTO = _mapper.Map<IntegrationCredentialDTO>(updated);

            return updatedDTO;
        }

        public async Task<IntegrationCredentialDTO> GetByIdAsync(int id)
        {
            var IntegrationCredential = await _repository.GetByIdAsync(id);
            if (IntegrationCredential is null) { throw new Exception("Credencial no se encontro"); }

            var IntegrationCredentialDTO = _mapper.Map<IntegrationCredentialDTO>(IntegrationCredential);
            return IntegrationCredentialDTO;
        }

        public async Task<PagedResult<IntegrationCredentialDTO>> GetDataTableAsync(int pageNumber, int pageSize, string? search)
        {
            var ( totalRecords, data ) = await _repository.GetDataTableAsync(pageNumber, pageSize, search);
            var dataDTO = _mapper.Map<List<IntegrationCredentialDTO>>(data);
            return new PagedResult<IntegrationCredentialDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = dataDTO
            };
        }

        /*public async Task<PagedResult<IntegrationCredentialListDTO>> GetListAsync(int pageNumber, int pageSize, string? search, string? type)
        {
            var (totalRecords, data) = await _repository.GetListAsync(pageNumber, pageSize, search, type);
            var dataDTO = _mapper.Map<List<IntegrationCredentialListDTO>>(data);
            return new PagedResult<IntegrationCredentialListDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = dataDTO
            };
        }*/


        public async Task<ToggleActiveResponseDTO> ToggleActiveAsync(int id, EstadoActualizadoRequestDTO infoRequestDto)
        {
            try
            {
                var currentUser = _authService.CurrentUser();

                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                {
                    throw new KeyNotFoundException($"Credencial con ID {id} no encontrado.");
                }
                existing.Active = infoRequestDto.Active;
                existing.UpdatedAtUser = currentUser.UserName;
                existing.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                existing.UpdatedAt = DateTime.UtcNow;
                var conductorActualizado = await _repository.UpdateAsync(existing);

                return _mapper.Map<ToggleActiveResponseDTO>(conductorActualizado);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<object> GetProviderToSelect()
        {
            return new List<object>
            {
                new { label = "Efact", value = "efact" }
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }


    }
}
