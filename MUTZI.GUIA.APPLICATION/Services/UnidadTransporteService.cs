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

        public async Task<List<TransportUnitDTO>> GetAllAsycn()
        {
            var transportes = await _repository.GetAllAsync();
            return _mapper.Map<List<TransportUnitDTO>>(transportes);
        }

        public async Task<List<TransportUnitListDTO>> GetListBySuggestion(string numeroDoc)
        {
            var transportes=await _repository.GetListBySuggestion(numeroDoc);
            return _mapper.Map<List<TransportUnitListDTO>>(transportes);
        }

        public async Task<PagedResult<TransportUnitDTO>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var (transporte, totalRecords)= await _repository.GetListAsync(pageNumber, pageSize, search);
            var transporteDto = _mapper.Map<List<TransportUnitDTO>>(transporte);
            return new PagedResult<TransportUnitDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = transporteDto
            };
        }

        public async Task<TransportUnitDTO?> GetByIdAsync(int id)
        {
            var transporte = await _repository.GetByIdAsync(id);
            return transporte == null ? null : _mapper.Map<TransportUnitDTO>(transporte);
        }

        public async Task<int> CreateAsync(TransportUnitCreateDTO createDTO)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var transporte = _mapper.Map<TransportUnit>(createDTO);
                transporte.CreatedAtUser = currentUser.UserName;
                transporte.CreatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}";
                transporte.CreatedAt = DateTime.UtcNow;
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

        public async Task<TransportUnitDTO> UpdateAsync(int id, TransportUnitUpdateDTO updateDTO)
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
                existingTransporte.UpdatedAtUser = currentUser.UserName;
                existingTransporte.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}";
                existingTransporte.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(existingTransporte);
                return _mapper.Map<TransportUnitDTO>(existingTransporte);
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
            existing.UpdatedAtUser = currentUser.UserName;
            existing.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}";
            existing.UpdatedAt = DateTime.UtcNow;

            var unidadTransporteActualizado = await _repository.UpdateAsync(existing);

            return _mapper.Map<EstadoActualizadoResponseDTO>(unidadTransporteActualizado);
        }
    }
}
