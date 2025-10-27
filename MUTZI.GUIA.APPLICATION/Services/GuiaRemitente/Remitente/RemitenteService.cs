using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Remitente;

namespace MUTZI.GUIA.APPLICATION.Services.GuiaRemitente.Remitente
{
    public class RemitenteService : IRemitenteService
    {
        private readonly IRemitenteRepository _repository;
        private readonly IMapper _mapper;
        public RemitenteService(IRemitenteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<RemitenteDTO>> GetAllAsync()
        {
            var remitentes = await _repository.GetAllAsync();
            return _mapper.Map<List<RemitenteDTO>>(remitentes);
        }
        public async Task<RemitenteDTO?> GetByIdAsync(int id)
        {
            var remitente = await _repository.GetByIdAsync(id);
            return remitente == null ? null : _mapper.Map<RemitenteDTO>(remitente);
        }

        public async Task<int> CreateAsync(RemitenteCreateDTO createDTO)
        {
            var remitente = _mapper.Map<DOMAIN.Entities.Remitente>(createDTO);
            await _repository.CreateAsync(remitente);
            return remitente.RemitenteId;
        }
        public async Task<RemitenteUpdateDTO> UpdateAsync(int id, RemitenteUpdateDTO updateDTO)
        {
            var existingRemitente = await _repository.GetByIdAsync(id);
            if (existingRemitente == null)
            {
                throw new KeyNotFoundException($"Remitente con ID {id} no encontrado.");
            }
            _mapper.Map(updateDTO, existingRemitente);
            await _repository.UpdateAsync(existingRemitente);
            return updateDTO;
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
