using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Conductor;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Services.GuiaRemitente.Conductor
{
    public class ConductorService : IConductorService
    {
        private readonly IConductorRepository _repository;
        private readonly IMapper _mapper;
        public ConductorService(IConductorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<ConductorDTO>> GetAllAsync()
        {
            var conductors = await _repository.GetAllAsync();
            return _mapper.Map<List<ConductorDTO>>(conductors);
        }
        public async Task<List<ConductorDTO>> GetListByName(string numeroDoc, string nombre)
        {
            var conductors = await _repository.GetListByName(numeroDoc, nombre);
            return _mapper.Map<List<ConductorDTO>>(conductors);
        }
        public async Task<PagedResult<ConductorDTO>> GetListAsync(int pageNumber, int pageSize)
        {
            var (conductor, totalRecords)=await _repository.GetListAsync(pageNumber, pageSize);
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
            var conductor = _mapper.Map<DOMAIN.Entities.Conductor>(createDTO);
            await _repository.CreateAsync(conductor);
            return conductor.ConductorId;
        }
        public async Task<ConductorDTO> UpdateAsync(int id, ConductorUpdateDTO updateDTO)
        {
            var existingConductor = await _repository.GetByIdAsync(id);
            if (existingConductor == null)
            {
                throw new KeyNotFoundException($"Conductor con ID {id} no encontrado.");
            }
            _mapper.Map(updateDTO, existingConductor);
            await _repository.UpdateAsync(existingConductor);
            return _mapper.Map<ConductorDTO>(existingConductor);
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task CancelAsync(int id, int status)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Conductor con ID {id} no encontrado.");
            }
            existing.Estado = status == 1 ? "activo" : "inactivo";
            existing.IdEstado = (sbyte?)status;
            await _repository.UpdateAsync(existing);
        }
    }
}
