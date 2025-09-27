using AutoMapper;
using GuiaRemision.DTOs.GuiaRemitente.UnidadTransporte;
using GuiaRemision.Repositories.GuiaRemitente.Transporte;

namespace GuiaRemision.Services.GuiaRemitente.Transporte
{
    public class TransporteService : ITransporteService
    {
        private readonly ITransporteRepository _repository;
        private readonly IMapper _mapper;
        public TransporteService(ITransporteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<UnidadTransporteDTO>> GetAllAsycn()
        {
            var transportes = await _repository.GetAllAsync();
            return _mapper.Map<List<UnidadTransporteDTO>>(transportes);
        }
        public async Task<UnidadTransporteDTO?> GetByIdAsync(int id)
        {
            var transporte = await _repository.GetByIdAsync(id);
            return transporte == null ? null : _mapper.Map<UnidadTransporteDTO>(transporte);
        }
        public async Task<int> CreateAsync(UnidadTransporteCreateDTO createDTO)
        {
            var transporte=_mapper.Map<Models.DBMS.Transporte>(createDTO);
            await _repository.CreateAsync(transporte);
            return transporte.TransporteId;
        }
        public async Task<UnidadTransporteUpdateDTO> UpdateAsync(int id, UnidadTransporteUpdateDTO updateDTO)
        {
            var existingTransporte = await _repository.GetByIdAsync(id);
            if (existingTransporte == null)
            {
                throw new KeyNotFoundException($"Transporte con ID {id} no encontrado.");
            }
            _mapper.Map(updateDTO, existingTransporte);
            await _repository.UpdateAsync(existingTransporte);
            return updateDTO;
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
