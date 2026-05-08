using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.DTOs.Transportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class TransportistaService: ITransportistaService
    {
        private readonly ITransportistaRepository _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public TransportistaService(
            ITransportistaRepository repository, 
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<List<TransportistaDTO>> GetAllAsycn()
        {
            var transportes = await _repository.GetAllAsync();
            return _mapper.Map<List<TransportistaDTO>>(transportes);
        }

        public async Task<List<TransportistaDTO>> GetListBySuggestion(string numeroDoc)
        {
            var transportes=await _repository.GetListBySuggestion(numeroDoc);
            return _mapper.Map<List<TransportistaDTO>>(transportes);
        }

        public async Task<PagedResult<TransportistaDTO>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var (transporte, totalRecords)= await _repository.GetListAsync(pageNumber, pageSize, search);
            var transporteDto = _mapper.Map<List<TransportistaDTO>>(transporte);
            return new PagedResult<TransportistaDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = transporteDto
            };
        }
        public async Task<TransportistaDTO?> GetByIdAsync(int id)
        {
            var transporte = await _repository.GetByIdAsync(id);
            return transporte == null ? null : _mapper.Map<TransportistaDTO>(transporte);
        }
        public async Task<int> CreateAsync(TransportistaCreateDTO createDTO)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var transporte = _mapper.Map<Transportista>(createDTO);
                transporte.UsuarioRegistro = currentUser.UserName;
                transporte.UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                transporte.FechaRegistro = DateTime.UtcNow;

                await _repository.CreateAsync(transporte);
                return transporte.TransportistaId;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("placa_UNIQUE")) { msg = "La placa ya se encuentra registrada."; }
                else if (msg.Contains("tarjeta_UNIQUE")) { msg = "Nro de tarjeta ya se encuentra registrado"; }
                throw new Exception(msg);
            }
        }
        public async Task<TransportistaDTO> UpdateAsync(int id, TransportistaUpdateDTO updateDTO)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var existingTransporte = await _repository.GetByIdAsync(id);
                if (existingTransporte == null)
                {
                    throw new KeyNotFoundException($"Transporte con ID {id} no encontrado.");
                }
                _mapper.Map(updateDTO, existingTransporte);
                existingTransporte.UsuarioModifico = currentUser.UserName;
                existingTransporte.UsuarioModificoNombre = $"{currentUser.FirstName} {currentUser.LastName}";
                existingTransporte.FechaModifico = DateTime.UtcNow;

                var transporteActualizado = await _repository.UpdateAsync(existingTransporte);
                return _mapper.Map<TransportistaDTO>(transporteActualizado);
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
            string _status = infoRequestDto.IdEstado switch
            {
                1 => "activo",
                0 => "inactivo",
                _ => throw new KeyNotFoundException("El nuevo estado solo acepta valores de '0' ó '1'"),
            };
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Unidad de Transporte con ID {id} no encontrado.");
            }
            existing.Estado = _status;
            existing.IdEstado = (sbyte?)infoRequestDto.IdEstado;
            existing.UsuarioModifico = currentUser.UserName;
            existing.UsuarioModificoNombre = $"{currentUser.FirstName} {currentUser.LastName}";
            existing.FechaModifico = DateTime.UtcNow;

            var transportistaActualizado = await _repository.UpdateAsync(existing);

            return _mapper.Map<EstadoActualizadoResponseDTO>(transportistaActualizado);
        }
    }
}
