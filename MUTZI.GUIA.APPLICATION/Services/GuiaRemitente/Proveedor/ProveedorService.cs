using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Proveedor;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Services.GuiaRemitente.Proveedor
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _repository;
        private readonly IMapper _mapper;
        public ProveedorService(IProveedorRepository repository, IMapper mapper)
        {
            _repository = repository;
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
        public async Task<PagedResult<ProveedorDTO>> GetListAsync(int pageNumber, int pageSize)
        {
            var (proveedor, totalRecords)=await _repository.GetListAsync(pageNumber, pageSize);
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
            var proveedor = _mapper.Map<DOMAIN.Entities.Proveedor>(createDTO);
            await _repository.CreateAsync(proveedor);
            return proveedor.ProveedorId;
        }
        public async Task<ProveedorDTO> UpdateAsync(int id, ProveedorUpdateDTO updateDTO)
        {
            var existingProveedor = await _repository.GetByIdAsync(id);
            if (existingProveedor == null)
            {
                throw new KeyNotFoundException($"Proveedor con ID {id} no encontrado.");
            }
            _mapper.Map(updateDTO, existingProveedor);
            await _repository.UpdateAsync(existingProveedor);
            return _mapper.Map<ProveedorDTO>(existingProveedor);
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task CancelAsync(int id, int status)
        {
            string _status = status switch
            {
                1 => "activo",
                2 => "inactivo",
                _ => throw new KeyNotFoundException("El nuevo estado solo acepta valores de '0' ó '1'"),
            };
            var existing=await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Proveedor con ID {id} no encontrado.");
            }
            existing.Estado = _status;
            existing.IdEstado = (sbyte?)status;
            await _repository.UpdateAsync(existing);
        }
    }
}
