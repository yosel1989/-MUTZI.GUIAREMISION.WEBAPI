using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class UnidadTransporteService : IUnidadTransporteService
    {
        private readonly IUnidadTransporteRepository _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public UnidadTransporteService(
            IUnidadTransporteRepository repository, 
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<List<UnidadTransporteDTO>> GetAllAsycn()
        {
            var transportes = await _repository.GetAllAsync();
            return _mapper.Map<List<UnidadTransporteDTO>>(transportes);
        }

        public async Task<List<UnidadTransporteListDTO>> GetListBySuggestion(string numeroDoc)
        {
            var transportes=await _repository.GetListBySuggestion(numeroDoc);
            return _mapper.Map<List<UnidadTransporteListDTO>>(transportes);
        }

        public async Task<PagedResult<UnidadTransporteDTO>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var (transporte, totalRecords)= await _repository.GetListAsync(pageNumber, pageSize, search);
            var transporteDto = _mapper.Map<List<UnidadTransporteDTO>>(transporte);
            return new PagedResult<UnidadTransporteDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = transporteDto
            };
        }

        public async Task<UnidadTransporteDTO?> GetByIdAsync(int id)
        {
            var transporte = await _repository.GetByIdAsync(id);
            return transporte == null ? null : _mapper.Map<UnidadTransporteDTO>(transporte);
        }

        public async Task<int> CreateAsync(UnidadTransporteCreateDTO createDTO)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var transporte = _mapper.Map<TransportUnit>(createDTO);
                transporte.UsuarioRegistro = currentUser.UserName;
                transporte.UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}";
                transporte.FechaRegistro = DateTime.UtcNow;
                await _repository.CreateAsync(transporte);
                return transporte.UnidadTransporteId;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("placa_UNIQUE")) { msg = "La placa ya se encuentra registrada."; }
                else if (msg.Contains("tarjeta_UNIQUE")) { msg = "Nro de tarjeta ya se encuentra registrado"; }
                throw new Exception(msg);
            }
        }

        public async Task<UnidadTransporteDTO> UpdateAsync(int id, UnidadTransporteUpdateDTO updateDTO)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var existingTransporte = await _repository.GetByIdAsync(id);
                if (existingTransporte == null)
                {
                    throw new KeyNotFoundException($"UnidadTransporte con ID {id} no encontrado.");
                }
                _mapper.Map(updateDTO, existingTransporte);
                existingTransporte.UsuarioModifico = currentUser.UserName;
                existingTransporte.UsuarioModificoNombre = $"{currentUser.FirstName} {currentUser.LastName}";
                existingTransporte.FechaModifico = DateTime.UtcNow;

                await _repository.UpdateAsync(existingTransporte);
                return _mapper.Map<UnidadTransporteDTO>(existingTransporte);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("placa_UNIQUE")) { msg = "La placa ya se encuentra registrada."; }
                else if (msg.Contains("tarjeta_UNIQUE")) { msg = "Nro de tarjeta ya se encuentra registrado"; }
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
            var currentUser = _authService.CurrentUser();
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Unidad de UnidadTransporte con ID {id} no encontrado.");
            }
            existing.Active = infoRequestDto.Active;
            existing.UsuarioModifico = currentUser.UserName;
            existing.UsuarioModificoNombre = $"{currentUser.FirstName} {currentUser.LastName}";
            existing.FechaModifico = DateTime.UtcNow;

            var unidadTransporteActualizado = await _repository.UpdateAsync(existing);

            return _mapper.Map<EstadoActualizadoResponseDTO>(unidadTransporteActualizado);
        }
    }
}
