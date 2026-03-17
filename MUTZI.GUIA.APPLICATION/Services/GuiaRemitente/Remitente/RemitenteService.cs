using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Remitente;
using MUTZI.GUIA.DOMAIN.Entities;

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
        public async Task<List<RemitenteListDTO>> GetAllAsync()
        {
            var remitentes = await _repository.GetAllAsync();
            return _mapper.Map<List<RemitenteListDTO>>(remitentes);
        }
        public async Task<List<RemitenteNameListDTO>> GetOnlynameAsync()
        {
            var remitentes = await _repository.GetOnlyNameAsync();
            return _mapper.Map<List<RemitenteNameListDTO>>(remitentes);
        }
        public async Task<List<RemitenteListDTO>> GetListBySuggestion(string numeroDoc)
        {
            var remitentes=await _repository.GetListBySuggestion(numeroDoc);
            return _mapper.Map<List<RemitenteListDTO>>(remitentes);
        }
        public async Task<PagedResult<RemitenteDTO>> GetListAsync(int pageNumber, int pageSize)
        {
            var (remitente, totalRecords) = await _repository.GetListAsync(pageNumber, pageSize);
            var remitenteDto=_mapper.Map<List<RemitenteDTO>>(remitente);
            return new PagedResult<RemitenteDTO>
            {
                PageNumber=pageNumber,
                PageSize=pageSize,
                TotalRecords=totalRecords,
                TotalPages=(int)Math.Ceiling(totalRecords/(double)pageSize),
                Data= remitenteDto
            };
        }
        public async Task<RemitenteDTO?> GetByIdAsync(int id)
        {
            var remitente = await _repository.GetByIdAsync(id);
            if (remitente == null) return null;
            return _mapper.Map<RemitenteDTO>(remitente);
        }
        public async Task<RemitenteForGuiaDTO?> GetByIdForGuiaAsync(int id, string tipoGuia)
        {
            var remitente = await _repository.GetByIdAsync(id);
            if (remitente == null) return null;

            var dto = _mapper.Map<RemitenteForGuiaDTO>(remitente);

            // Calcular serie completa según tipo de guía
            var prefix = string.Equals(tipoGuia, "TRANSPORTISTA", StringComparison.OrdinalIgnoreCase) ? "V" : "T";
            var serieBase = remitente.Serie?.Trim();
            if (!string.IsNullOrEmpty(serieBase))
            {
                var serieCompleta = prefix + serieBase;

                // Obtener la última guía registrada para esa serie
                var lastGuia = await _repository.GetLastBySerieAsync(serieCompleta);

                // Calcular el siguiente correlativo (si no existe, iniciar en 1)
                int nextCorrelativo = 1;
                if (lastGuia != null && !string.IsNullOrWhiteSpace(lastGuia.Correlativo))
                {
                    if (int.TryParse(lastGuia.Correlativo, out int lastCorr))
                        nextCorrelativo = lastCorr + 1;
                }

                // Formatear correlativo a 8 dígitos y construir número de guía
                string correlativoStr = nextCorrelativo.ToString("D8");

                dto.NuevaSerie = serieCompleta;
                dto.NuevoCorrelativo = nextCorrelativo;
                dto.NuevoNumeroGuia = $"{serieCompleta}-{correlativoStr}";
            }

            return dto;
        }
        public async Task<int> CreateAsync(RemitenteCreateDTO createDTO)
        {
            var existe = await _repository.GetBySerieAsync(createDTO.Ruc, createDTO.Serie);
            if (existe) { throw new Exception("La serie ya se encuentra registrado con este RUC."); }
            try
            {
                var remitente = _mapper.Map<DOMAIN.Entities.Remitente>(createDTO);
                await _repository.CreateAsync(remitente);
                return remitente.RemitenteId;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("razonSocial_UNIQUE")) { msg = "La razón social social o sucursal del remitente ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }
        public async Task<RemitenteDTO> UpdateAsync(int id, RemitenteUpdateDTO updateDTO)
        {
            try
            {
                var existingRemitente = await _repository.GetByIdAsync(id);
                if (existingRemitente == null)
                {
                    throw new KeyNotFoundException($"Remitente con ID {id} no encontrado.");
                }
                _mapper.Map(updateDTO, existingRemitente);
                await _repository.UpdateAsync(existingRemitente);
                return _mapper.Map<RemitenteDTO>(existingRemitente);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("razonSocial_UNIQUE")) { msg = "La razón social social o sucursal del remitente ya se encuentra registrado."; }
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
                if (msg.Contains("FOREIGN KEY")) { msg = "No se puede eliminar porque existen registros asociados a esta tabla."; }
                throw new Exception(msg);
            }
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
                throw new KeyNotFoundException($"Remitente con ID {id} no encontrado.");
            }
            existing.Estado = _status;
            existing.IdEstado = (sbyte?)infoRequestDto.IdEstado;
            existing.EmpleadoIdedicion = infoRequestDto.EditedEmployeeId;
            existing.EmpleadoNombreEdicion = infoRequestDto.EditedEmployeeName;
            await _repository.UpdateAsync(existing);

            return new UpdatedStatusDto
            {
                Id = existing.RemitenteId,
                IdEstado = (Int32)existing.IdEstado,
                Estado = existing.Estado,
                Detalle = $"Remitente con ID {existing.RemitenteId} actualizado a estado '{existing.Estado}'."
            };
        }
    }
}
