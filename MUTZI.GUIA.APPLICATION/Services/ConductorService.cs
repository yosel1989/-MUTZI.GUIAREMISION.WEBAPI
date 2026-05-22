using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Conductor;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class ConductorService : IConductorService
    {
        private readonly IConductorRepository _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public ConductorService(IConductorRepository repository, IMapper mapper, IAuthService authService)
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<List<ConductorDTO>> GetAllAsync()
        {
            var conductors = await _repository.GetAllAsync();
            return _mapper.Map<List<ConductorDTO>>(conductors);
        }
        public async Task<List<ConductorListDTO>> GetListBySuggestion(string numeroDoc)
        {
            var conductors = await _repository.GetListBySuggestion(numeroDoc);
            return _mapper.Map<List<ConductorListDTO>>(conductors);
        }
        public async Task<PagedResult<ConductorDTO>> GetListAsync(int pageNumber, int pageSize, string? search)
        {

            var (conductor, totalRecords) = await _repository.GetListAsync(pageNumber, pageSize, search);
            var conductorDTO = _mapper.Map<List<ConductorDTO>>(conductor);
            return new PagedResult<ConductorDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = conductorDTO
            };
        }
        public async Task<ConductorDTO?> GetByIdAsync(int id)
        {
            var conductor = await _repository.GetByIdAsync(id);
            return conductor == null ? null : _mapper.Map<ConductorDTO>(conductor);
        }
        public async Task<ConductorDTO?> GetByNumeroDocumentoAsync(string numeroDocumento)
        {
            var conductor = await _repository.GetByNumeroDocumentoAsync(numeroDocumento);
            return conductor == null ? null : _mapper.Map<ConductorDTO>(conductor);
        }
        public async Task<int> CreateAsync(ConductorCreateDTO createDTO)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var conductor = _mapper.Map<Conductor>(createDTO);
                conductor.UsuarioRegistro = currentUser.UserName;
                conductor.UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                conductor.FechaRegistro = DateTime.UtcNow;
                await _repository.CreateAsync(conductor);
                return conductor.ConductorId;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("numeroDocumento_UNIQUE")) { msg = "El número de documento ya se encuentra registrado."; }
                else if (msg.Contains("licencia_UNIQUE")) { msg = "Nro Licencia ya se encuentra registrado"; }
                    throw new Exception(msg);
            }
        }
        public async Task<ConductorDTO> UpdateAsync(int id, ConductorUpdateDTO updateDTO)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var existingConductor = await _repository.GetByIdAsync(id);
                if (existingConductor == null)
                {
                    throw new KeyNotFoundException($"Conductor con ID {id} no encontrado.");
                }
                _mapper.Map(updateDTO, existingConductor);
                existingConductor.UsuarioModifico = currentUser.UserName;
                existingConductor.UsuarioModificoNombre = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                existingConductor.FechaModifico = DateTime.UtcNow;

                var conductorActualizado = await _repository.UpdateAsync(existingConductor);
                return _mapper.Map<ConductorDTO>(conductorActualizado);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("numeroDocumento_UNIQUE")) { msg = "El número de documento ya se encuentra registrado."; }
                else if (msg.Contains("licencia_UNIQUE")) { msg = "Nro Licencia ya se encuentra registrado"; }
                throw new Exception(msg);
            }
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
        public async Task<EstadoActualizadoResponseDTO> CambiarEstadoAsync(int id, EstadoActualizadoRequestDTO infoRequestDto)
        {
            try
            {
                var currentUser = _authService.CurrentUser();

                string _status = infoRequestDto.EstadoId switch
                {
                    1 => "activo",
                    0 => "inactivo",
                    _ => throw new KeyNotFoundException("El nuevo estado solo acepta valores de '0' ó '1'"),
                };
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                {
                    throw new KeyNotFoundException($"Conductor con ID {id} no encontrado.");
                }
                existing.Estado = _status;
                existing.EstadoId = (sbyte)infoRequestDto.EstadoId;
                existing.UsuarioModifico = currentUser.UserName;
                existing.UsuarioModificoNombre = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                existing.FechaModifico = DateTime.UtcNow;
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
