using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.UnidadTransporte;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Transporte;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Transporte;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Transporte;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;

namespace MUTZI.GUIA.APPLICATION.Services.GuiaRemitente.Transporte
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
        public async Task<PagedResult<UnidadTransporteDTO>> GetListAsync(int pageNumber, int pageSize)
        {
            var (transporte, totalRecords)= await _repository.GetListAsync(pageNumber, pageSize);
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
            var transporte=_mapper.Map<DOMAIN.Entities.Transporte>(createDTO);
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
