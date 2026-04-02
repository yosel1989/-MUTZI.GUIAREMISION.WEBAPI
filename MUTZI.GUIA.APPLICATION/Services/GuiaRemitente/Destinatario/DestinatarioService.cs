using AutoMapper;
using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Destinatario;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Services.GuiaRemitente.Destinatario
{
    public class DestinatarioService : IDestinatarioService
    {
        private readonly IDestinatarioRepository _repository;
        private readonly IMapper _mapper;
        public DestinatarioService(IDestinatarioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<DestinatarioDTO>> GetAllAsync()
        {
            var destinatarios = await _repository.GetAllAsync();
            return _mapper.Map<List<DestinatarioDTO>>(destinatarios);
        }
        public async Task<List<DestinatarioListDTO>> GetListBySuggestion(string numeroDoc)
        {
            var destinatarios=await _repository.GetListBySuggestion(numeroDoc);
            return _mapper.Map<List<DestinatarioListDTO>>(destinatarios);
        }
        public async Task<PagedResult<DestinatarioDTO>> GetListAsync(int pageNumber, int pageSize, IQueryCollection q)
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
            var (destinatario, totalRecords)=await _repository.GetListAsync(pageNumber, pageSize, request);
            var destinatarioDTO = _mapper.Map<List<DestinatarioDTO>>(destinatario);
            return new PagedResult<DestinatarioDTO>{
                PageNumber=pageNumber,
                PageSize=pageSize,
                TotalRecords=totalRecords,
                TotalPages=(int)Math.Ceiling(totalRecords/(double)pageSize),
                Data=destinatarioDTO
            };
        }
        public async Task<DestinatarioDTO?> GetByIdAsync(int id)
        {
            var destinatario = await _repository.GetByIdAsync(id);
            return destinatario == null ? null : _mapper.Map<DestinatarioDTO>(destinatario);
        }
        public async Task<DestinatarioDTO?> GetByNumeroDocumentoAsync(string numeroDocumento)
        {
            var destinatario = await _repository.GetByNumeroDocumentoAsync(numeroDocumento);
            return destinatario == null ? null : _mapper.Map<DestinatarioDTO>(destinatario);
        }
        public async Task<int> CreateAsync(DestinatarioCreateDTO createDTO)
        {
            try
            {
                var destinatario = _mapper.Map<DOMAIN.Entities.Destinatario>(createDTO);
                await _repository.CreateAsync(destinatario);
                return destinatario.DestinatarioId;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("numeroDocumento_UNIQUE")) { msg = "El número de documento ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }
        public async Task<DestinatarioDTO> UpdateAsync(int id, DestinatarioUpdateDTO updateDTO)
        {
            try
            {
                var existingDestinatario = await _repository.GetByIdAsync(id);
                if (existingDestinatario == null)
                {
                    throw new KeyNotFoundException($"Destinatario con ID {id} no encontrado.");
                }
                _mapper.Map(updateDTO, existingDestinatario);
                await _repository.UpdateAsync(existingDestinatario);
                return _mapper.Map<DestinatarioDTO>(existingDestinatario);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("numeroDocumento_UNIQUE")) { msg = "El número de documento ya se encuentra registrado."; }
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
                throw new KeyNotFoundException($"Destinatario con ID {id} no encontrado.");
            }
            existing.Estado = _status;
            existing.IdEstado = (sbyte?)infoRequestDto.IdEstado;
            existing.EmpleadoIdedicion = infoRequestDto.EditedEmployeeId;
            existing.EmpleadoNombreEdicion = infoRequestDto.EditedEmployeeName;
            await _repository.UpdateAsync(existing);

            return new UpdatedStatusDto
            {
                Id = existing.DestinatarioId,
                IdEstado = (Int32)existing.IdEstado,
                Estado = existing.Estado,
                Detalle = $"Destinatario con ID {existing.DestinatarioId} ha sido actualizado a estado '{existing.Estado}'."
            };
        }
    }
}
