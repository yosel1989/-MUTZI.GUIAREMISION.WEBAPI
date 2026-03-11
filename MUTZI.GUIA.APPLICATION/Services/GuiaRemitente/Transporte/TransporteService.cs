using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.UnidadTransporte;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Transporte;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Transporte;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Transporte;
using MUTZI.GUIA.DOMAIN.Entities;

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
        public async Task<List<TransporteListDTO>> GetListByName(string numeroDoc, string nombre)
        {
            var transportes=await _repository.GetListByName(numeroDoc, nombre);
            return _mapper.Map<List<TransporteListDTO>>(transportes);
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
        public async Task<UnidadTransporteDTO> UpdateAsync(int id, UnidadTransporteUpdateDTO updateDTO)
        {
            var existingTransporte = await _repository.GetByIdAsync(id);
            if (existingTransporte == null)
            {
                throw new KeyNotFoundException($"Transporte con ID {id} no encontrado.");
            }
            _mapper.Map(updateDTO, existingTransporte);
            await _repository.UpdateAsync(existingTransporte);
            return _mapper.Map<UnidadTransporteDTO>(existingTransporte);
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
                throw new KeyNotFoundException($"Unidad de Transporte con ID {id} no encontrado.");
            }
            existing.Estado = status == 1 ? "activo" : "inactivo";
            existing.IdEstado = (sbyte?)status;
            await _repository.UpdateAsync(existing);
        }
    }
}
