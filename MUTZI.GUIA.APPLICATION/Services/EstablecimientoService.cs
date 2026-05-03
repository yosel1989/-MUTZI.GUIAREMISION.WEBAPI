using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class EstablecimientoService : IEstablecimientoService
    {
        private readonly IEstablecimientoRepository _repository;
        private readonly IMapper _mapper;
        public EstablecimientoService(IEstablecimientoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<EstablecimientoListDTO>> GetAllByRucToSelectAsync(string ruc)
        {
            var data = await _repository.GetAllByRucToSelectAsync(ruc);
            return _mapper.Map<List<EstablecimientoListDTO>>(data);
        }

        public async Task<List<EstablecimientoListToModalDTO>> GetListBySuggestionAsync(string ruc, string? search)
        {
            var data = await _repository.GetListBySuggestion(ruc, search);
            return _mapper.Map<List<EstablecimientoListToModalDTO>>(data);
        }

        public async Task<EstablecimientoDTO?> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            return _mapper.Map<EstablecimientoDTO>(data);
        }

        public async Task<PagedResult<EstablecimientoDTO>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var (data, totalRecords) = await _repository.GetListAsync(pageNumber, pageSize, search);

            return new PagedResult<EstablecimientoDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = _mapper.Map<List<EstablecimientoDTO>>(data)
            };
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
                throw new KeyNotFoundException($"Conductor con ID {id} no encontrado.");
            }
            existing.Estado = _status;
            existing.IdEstado = (sbyte)infoRequestDto.IdEstado;
            existing.EmpleadoIdedicion = infoRequestDto.EditedEmployeeId;
            existing.EmpleadoNombreEdicion = infoRequestDto.EditedEmployeeName;
            await _repository.UpdateAsync(existing);

            return new UpdatedStatusDto
            {
                Id = existing.EstablecimientoId,
                IdEstado = Convert.ToInt32(existing.IdEstado),
                Estado = existing.Estado,
                FechaEdicion = existing.FechaEdicion,
                EmpleadoNombreEdicion = infoRequestDto.EditedEmployeeName,
                Detalle = $"Establecimiento con ID {existing.EstablecimientoId} ha sido actualizado a estado '{existing.Estado}'."
            };
        }

        public async Task<int> CreateAsync(EstablecimientoCreateDTO createDTO)
        {
            var existe = await _repository.GetBySerieAsync(createDTO.Ruc, createDTO.Serie);
            if (existe) { throw new Exception("La serie ya se encuentra registrado con este RUC."); }
            try
            {
                var establecimiento = _mapper.Map<Establecimiento>(createDTO);
                await _repository.CreateAsync(establecimiento);
                return establecimiento.EstablecimientoId;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("razonSocial_UNIQUE")) { msg = "La razón social social o sucursal del establecimiento ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }

        public async Task<EstablecimientoDTO> UpdateAsync(int id, EstablecimientoUpdateDTO updateDTO)
        {
            try
            {
                var existingEstablecimiento = await _repository.GetByIdAsync(id);
                if (existingEstablecimiento == null)
                {
                    throw new KeyNotFoundException($"Establecimiento con ID {id} no encontrado.");
                }
                if (updateDTO.Serie != existingEstablecimiento.Serie)
                {
                    var existe = await _repository.GetBySerieAsync(updateDTO.Ruc, updateDTO.Serie);
                    if (existe) { throw new Exception("La serie ya se encuentra registrado con este RUC."); }
                }
                var establecimientoDomain = _mapper.Map(updateDTO, existingEstablecimiento);
                var establecimientoActualizado = await _repository.UpdateAsync(establecimientoDomain);
                return _mapper.Map<EstablecimientoDTO>(establecimientoActualizado);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("razonSocial_UNIQUE")) { msg = "La razón social social o sucursal del establecimiento ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }

        public async Task<EstablecimientoRemitenteDTO?> GetByIdForGuiaAsync(int id, string tipoGuia)
        {
            try
            {
                var establecimiento = await _repository.GetByIdAsync(id);
                if (establecimiento == null) return null;

                var dto = _mapper.Map<EstablecimientoRemitenteDTO>(establecimiento);

                // Calcular serie completa según tipo de guía
                var prefix = string.Equals(tipoGuia, "TRANSPORTISTA", StringComparison.OrdinalIgnoreCase) ? "V" : "T";
                var serieBase = establecimiento.Serie?.Trim();
                if (!string.IsNullOrEmpty(serieBase))
                {
                    var serieCompleta = prefix + serieBase;

                    // Obtener la última guía registrada para esa serie
                    var lastGuia = await _repository.GetLastBySerieAsync(serieCompleta, establecimiento.Ruc);

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
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
