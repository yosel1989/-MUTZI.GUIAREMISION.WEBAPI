using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using Org.BouncyCastle.Asn1.Ocsp;

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
            existing.IdEstado = (sbyte?)infoRequestDto.IdEstado;
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
                _mapper.Map(updateDTO, existingEstablecimiento);
                await _repository.UpdateAsync(existingEstablecimiento);
                return _mapper.Map<EstablecimientoDTO>(existingEstablecimiento);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("razonSocial_UNIQUE")) { msg = "La razón social social o sucursal del establecimiento ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }
    }
}
