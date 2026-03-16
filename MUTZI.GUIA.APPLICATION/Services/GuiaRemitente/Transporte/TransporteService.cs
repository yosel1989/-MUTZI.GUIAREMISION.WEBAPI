using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
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
        public async Task<List<TransporteListDTO>> GetListBySuggestion(string numeroDoc)
        {
            var transportes=await _repository.GetListBySuggestion(numeroDoc);
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
            try
            {
                var transporte = _mapper.Map<DOMAIN.Entities.Transporte>(createDTO);
                await _repository.CreateAsync(transporte);
                return transporte.TransporteId;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("placa_UNIQUE")) { msg = "La placa ya se encuentra registrada."; }
                else if (msg.Contains("tarjeta_UNIQUE")) { msg = "Nro de tarjeta ya se encuentra registrado"; }
                throw new Exception(msg);
            }
        }
        public async Task<UnidadTransporteDTO> UpdateAsync(int id, UnidadTransporteUpdateDTO updateDTO)
        {
            try
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
            await _repository.DeleteAsync(id);
        }
        public async Task<UpdatedStatusDto> CambiarEstadoAsync(int id, InfoRequestDTO infoRequestDto)
        {
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
            existing.EmpleadoIdedicion = infoRequestDto.EditedEmployeeId;
            existing.EmpleadoNombreEdicion = infoRequestDto.EditedEmployeeName;
            await _repository.UpdateAsync(existing);

            return new UpdatedStatusDto
            {
                Id = existing.TransporteId,
                IdEstado = (Int32)existing.IdEstado,
                Estado = existing.Estado,
                FechaEdicion = existing.FechaEdicion,
                EmpleadoNombreEdicion = infoRequestDto.EditedEmployeeName,
                Detalle = $"Unidad de Transporte con ID {existing.TransporteId} ha sido actualizado a estado '{existing.Estado}'."
            };
        }
    }
}
