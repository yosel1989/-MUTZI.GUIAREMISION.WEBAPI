using AutoMapper;
using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Conductor;
using MUTZI.GUIA.DOMAIN.Entities;
using Org.BouncyCastle.Asn1.Ocsp;

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
        public async Task<List<ConductorListDTO>> GetListBySuggestion(string numeroDoc)
        {
            var conductors = await _repository.GetListBySuggestion(numeroDoc);
            return _mapper.Map<List<ConductorListDTO>>(conductors);
        }
        public async Task<PagedResult<ConductorDTO>> GetListAsync(int pageNumber, int pageSize, IQueryCollection q)
        {
            var request = new DataTableDTO { Columns = new List<RequestFiltroDTO>() };
            var searchValue = q[$"columns[{0}][search][value]"];
            if (!string.IsNullOrEmpty(searchValue))
            {
                request.Columns.Add(new RequestFiltroDTO
                {
                    Data = q[$"columns[{0}][data]"],
                    Search = new SearchDTO
                    {
                        Value = searchValue
                    }
                });
            }
            var (conductor, totalRecords)=await _repository.GetListAsync(pageNumber, pageSize, request);
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
            try
            {
                var conductor = _mapper.Map<DOMAIN.Entities.Conductor>(createDTO);
                await _repository.CreateAsync(conductor);
                return conductor.ConductorId;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("numeroDocumento_UNIQUE")) { msg = "El número de documento ya se encuentra registrado."; }
                else if (msg.Contains("licencia_UNIQUE")) { msg = "Nro Licencia ya se encuentra registrado"; }
                    throw new Exception(msg);
            }
        }
        public async Task<ConductorDTO> UpdateAsync(int id, ConductorUpdateDTO updateDTO)
        {
            try
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
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("numeroDocumento_UNIQUE")) { msg = "El número de documento ya se encuentra registrado."; }
                else if (msg.Contains("licencia_UNIQUE")) { msg = "Nro Licencia ya se encuentra registrado"; }
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
                if (msg.Contains("FOREIGN KEY")) { msg = "No se puede eliminar porque existen registros asociados"; }
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
                throw new KeyNotFoundException($"Conductor con ID {id} no encontrado.");
            }
            existing.Estado = _status;
            existing.IdEstado = (sbyte?)infoRequestDto.IdEstado;
            existing.EmpleadoIdedicion = infoRequestDto.EditedEmployeeId;
            existing.EmpleadoNombreEdicion = infoRequestDto.EditedEmployeeName;
            await _repository.UpdateAsync(existing);

            return new UpdatedStatusDto
            {
                Id = existing.ConductorId,
                IdEstado = Convert.ToInt32(existing.IdEstado),
                Estado = existing.Estado,
                Detalle = $"Conductor con ID {existing.ConductorId} ha sido actualizado a estado '{existing.Estado}'."
            };
        }
    }
}
