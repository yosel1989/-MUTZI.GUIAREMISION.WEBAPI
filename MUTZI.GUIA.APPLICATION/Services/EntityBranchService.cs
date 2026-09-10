using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class EntityBranchService : IEntityBranchService
    {
        private readonly IEntityBranchRepository _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public EntityBranchService(
            IEntityBranchRepository repository,
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<List<EntityBranchListDTO>> GetAllByEntityToSelectAsync(int entityId)
        {
            var data = await _repository.GetAllByEntityToSelectAsync(entityId);
            return _mapper.Map<List<EntityBranchListDTO>>(data);
        }

        public async Task<List<EntityBranchToSelectDTO>> GetAllByEntityToSelect2Async(int entityId)
        {
            var data = await _repository.GetAllByEntityToSelect2Async(entityId);
            return _mapper.Map<List<EntityBranchToSelectDTO>>(data);
        }

        public async Task<List<EntityBranchListToModalDTO>> GetListBySuggestionAsync(int entityId, string? search)
        {
            var data = await _repository.GetListBySuggestion(entityId, search);
            return _mapper.Map<List<EntityBranchListToModalDTO>>(data);
        }

        public async Task<EntityBranchDTO?> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            return _mapper.Map<EntityBranchDTO>(data);
        }

        public async Task<PagedResult<EntityBranchDTO>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var (data, totalRecords) = await _repository.GetListAsync(pageNumber, pageSize, search);

            return new PagedResult<EntityBranchDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = _mapper.Map<List<EntityBranchDTO>>(data)
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<EstadoActualizadoResponseDTO> CambiarEstadoAsync(int id, EstadoActualizadoRequestDTO infoRequestDto)
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
            var establecimientoActualizado = await _repository.UpdateAsync(existing);

            return _mapper.Map<EstadoActualizadoResponseDTO>(establecimientoActualizado);
        }

        public async Task<int> CreateAsync(EntityBranchCreateDTO createDTO)
        {
            var currentUser = _authService.CurrentUser();
            var existe = await _repository.GetBySerieAsync(createDTO.EntityId, createDTO.Serie);
            if (existe) { throw new Exception("La serie ya se encuentra registrada a esa entidad"); }
            try
            {
                var establecimiento = _mapper.Map<EntityBranch>(createDTO);
                establecimiento.CreatedAtUser = currentUser.UserName;
                establecimiento.CreatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                establecimiento.CreatedAt = DateTime.UtcNow;
                await _repository.CreateAsync(establecimiento);
                return establecimiento.EntityBranchId;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("razonSocial_UNIQUE")) { msg = "La razón social social o sucursal del establecimiento ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }

        public async Task<EntityBranchDTO> UpdateAsync(int id, EntityBranchUpdateDTO updateDTO)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var existingEstablecimiento = await _repository.GetByIdAsync(id);
                if (existingEstablecimiento == null)
                {
                    throw new KeyNotFoundException($"EntityBranch con ID {id} no encontrado.");
                }
                if (updateDTO.Serie != existingEstablecimiento.Serie)
                {
                    var existe = await _repository.GetBySerieAsync(updateDTO.EntityId, updateDTO.Serie);
                    if (existe) { throw new Exception("La serie ya se encuentra registrada con está entidad"); }
                }


                existingEstablecimiento.EntityId = updateDTO.EntityId;
                existingEstablecimiento.Description = updateDTO.Description;
                existingEstablecimiento.Area = updateDTO.Area;
                existingEstablecimiento.UbigeoId = updateDTO.UbigeoId;
                existingEstablecimiento.Address = updateDTO.Address;
                existingEstablecimiento.Email = updateDTO.Email;
                existingEstablecimiento.Serie = updateDTO.Serie;
                existingEstablecimiento.CodeSunat = updateDTO.CodeSunat;
                existingEstablecimiento.Type = updateDTO.Type;
                existingEstablecimiento.IsMain = updateDTO.IsMain;
                existingEstablecimiento.UpdatedAt = DateTime.UtcNow;
                existingEstablecimiento.UpdatedAtUser = currentUser.UserName;
                existingEstablecimiento.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                var establecimientoActualizado = await _repository.UpdateAsync(existingEstablecimiento);
                return _mapper.Map<EntityBranchDTO>(establecimientoActualizado);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("razonSocial_UNIQUE")) { msg = "La razón social social o sucursal del establecimiento ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }

        public async Task<EstablecimientoRemitenteDTO?> GetByIdForGuiaAsync(int id, string tipoGuia)
        {
            try
            {
                var establecimiento = await _repository.GetByIdAsync(id);
                if (establecimiento == null) return null;

                var dto = _mapper.Map<EstablecimientoRemitenteDTO>(establecimiento);

                // Calcular serie completa según tipo de guía
                var prefix = string.Equals(tipoGuia, "TRANSPORTISTA", StringComparison.OrdinalIgnoreCase) ? "V" : "T";
                var serieBase = establecimiento.Serie?.Trim();
                if (!string.IsNullOrEmpty(serieBase))
                {
                    var serieCompleta = prefix + serieBase;

                    // Obtener la última guía registrada para esa serie
                    var lastGuia = await _repository.GetLastBySerieAsync(serieCompleta, establecimiento.EntityId);

                    // Calcular el siguiente correlativo (si no existe, iniciar en 1)
                    int nextCorrelativo = 1;
                    if (lastGuia != null && !string.IsNullOrWhiteSpace(lastGuia.Correlativo))
                    {
                        if (int.TryParse(lastGuia.Correlativo, out int lastCorr))
                            nextCorrelativo = lastCorr + 1;
                    }

                    // Formatear correlativo a 8 dígitos y construir número de guía
                    string correlativoStr = nextCorrelativo.ToString("D8");

                    dto.NewSerie = serieCompleta;
                    dto.NuevoCorrelativo = nextCorrelativo;
                    dto.NuevoNumeroGuia = $"{serieCompleta}-{correlativoStr}";
                }

                return dto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
