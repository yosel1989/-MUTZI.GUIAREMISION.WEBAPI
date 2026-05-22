using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Proveedor;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Proveedor;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public ProveedorService(
            IProveedorRepository repository, 
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<List<ProveedorDTO>> GetAllAsync()
        {
            var proveedores = await _repository.GetAllAsync();
            return _mapper.Map<List<ProveedorDTO>>(proveedores);
        }

        public async Task<List<ProveedorListDTO>> GetListBySuggestion(string numeroDoc)
        {
            var proveedores = await _repository.GetListBySuggestion(numeroDoc);
            return _mapper.Map<List<ProveedorListDTO>>(proveedores);
        }

        public async Task<PagedResult<ProveedorDTO>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var (proveedor, totalRecords)=await _repository.GetListAsync(pageNumber, pageSize, search);
            var proveedorDTO = _mapper.Map<List<ProveedorDTO>>(proveedor);
            return new PagedResult<ProveedorDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = proveedorDTO
            };
        }

        public async Task<ProveedorDTO?> GetByIdAsync(int id)
        {
            var proveedor = await _repository.GetByIdAsync(id);
            return proveedor == null ? null : _mapper.Map<ProveedorDTO>(proveedor);
        }

        public async Task<ProveedorDTO?> GetByNumeroDocumentoAsync(string numeroDocumento)
        {
            var proveedor = await _repository.GetByNumeroDocumentoAsync(numeroDocumento);
            return proveedor == null ? null : _mapper.Map<ProveedorDTO>(proveedor);
        }

        public async Task<int> CreateAsync(ProveedorCreateDTO createDTO)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var proveedor = _mapper.Map<Proveedor>(createDTO);
                proveedor.UsuarioRegistro = currentUser.UserName;
                proveedor.UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                proveedor.FechaRegistro = DateTime.UtcNow;
                await _repository.CreateAsync(proveedor);
                return proveedor.ProveedorId;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("numeroDocumento_UNIQUE")) { msg = "El número de documento ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }
        public async Task<ProveedorDTO> UpdateAsync(int id, ProveedorUpdateDTO updateDTO)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var existingProveedor = await _repository.GetByIdAsync(id);
                if (existingProveedor == null)
                {
                    throw new KeyNotFoundException($"Proveedor con ID {id} no encontrado.");
                }
                _mapper.Map(updateDTO, existingProveedor);
                existingProveedor.UsuarioModifico = currentUser.UserName;
                existingProveedor.UsuarioModificoNombre = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                existingProveedor.FechaModifico = DateTime.UtcNow;
                await _repository.UpdateAsync(existingProveedor);
                return _mapper.Map<ProveedorDTO>(existingProveedor);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("numeroDocumento_UNIQUE")) { msg = "El número de documento ya se encuentra registrado."; }
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
            string _status = infoRequestDto.EstadoId switch
            {
                1 => "activo",
                0 => "inactivo",
                _ => throw new KeyNotFoundException("El nuevo estado solo acepta valores de '0' ó '1'"),
            };
            var existing=await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Proveedor con ID {id} no encontrado.");
            }
            existing.Estado = _status;
            existing.EstadoId = (sbyte?)infoRequestDto.EstadoId;
            existing.UsuarioModifico = currentUser.UserName;
            existing.UsuarioModificoNombre = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
            existing.FechaModifico = DateTime.UtcNow;
            var proveedorActualizado = await _repository.UpdateAsync(existing);

            return _mapper.Map<EstadoActualizadoResponseDTO>(proveedorActualizado);
        }
    }
}
