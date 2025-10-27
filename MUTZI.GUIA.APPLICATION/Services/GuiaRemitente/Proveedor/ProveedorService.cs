using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Proveedor;

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
        public async Task<ProveedorUpdateDTO> UpdateAsync(int id, ProveedorUpdateDTO updateDTO)
        {
            var existingProveedor = await _repository.GetByIdAsync(id);
            if (existingProveedor == null)
            {
                throw new KeyNotFoundException($"Proveedor con ID {id} no encontrado.");
            }
            _mapper.Map(updateDTO, existingProveedor);
            await _repository.UpdateAsync(existingProveedor);
            return updateDTO;
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
