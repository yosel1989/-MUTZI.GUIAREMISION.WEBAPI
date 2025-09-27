using AutoMapper;
using GuiaRemision.DTOs.GuiaRemitente.Conductor;
using GuiaRemision.Repositories.GuiaRemitente.Conductor;

namespace GuiaRemision.Services.GuiaRemitente.Conductor
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
        public async Task<ConductorDTO?> GetByIdAsync(int id)
        {
            var conductor = await _repository.GetByIdAsync(id);
            return conductor == null ? null : _mapper.Map<ConductorDTO>(conductor);
        }
        public async Task<int> CreateAsync(ConductorCreateDTO createDTO)
        {
            var conductor = _mapper.Map<Models.DBMS.Conductor>(createDTO);
            await _repository.CreateAsync(conductor);
            return conductor.ConductorId;
        }
        public async Task<ConductorUpdateDTO> UpdateAsync(int id, ConductorUpdateDTO updateDTO)
        {
            var existingConductor = await _repository.GetByIdAsync(id);
            if (existingConductor == null)
            {
                throw new KeyNotFoundException($"Conductor con ID {id} no encontrado.");
            }
            _mapper.Map(updateDTO, existingConductor);
            await _repository.UpdateAsync(existingConductor);
            return updateDTO;
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
