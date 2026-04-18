using AutoMapper;
using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.RemiteTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.Transportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Transportista;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Services.Transportista
{
    public class TransportistaService: ITransportistaService
    {
        private readonly ITransportistaRepository _repository;
        private readonly IMapper _mapper;
        public TransportistaService(ITransportistaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<RemiteTransportistaDTO>> GetAllAsycn()
        {
            var transportes = await _repository.GetAllAsync();
            return _mapper.Map<List<RemiteTransportistaDTO>>(transportes);
        }
        public async Task<List<RemiteTransportistaDTO>> GetListBySuggestion(string numeroDoc)
        {
            var transportes=await _repository.GetListBySuggestion(numeroDoc);
            return _mapper.Map<List<RemiteTransportistaDTO>>(transportes);
        }
        public async Task<PagedResult<RemiteTransportistaDTO>> GetListAsync(int pageNumber, int pageSize, IQueryCollection q)
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
            var (transporte, totalRecords)= await _repository.GetListAsync(pageNumber, pageSize, request);
            var transporteDto = _mapper.Map<List<RemiteTransportistaDTO>>(transporte);
            return new PagedResult<RemiteTransportistaDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = transporteDto
            };
        }
        public async Task<RemiteTransportistaDTO?> GetByIdAsync(int id)
        {
            var transporte = await _repository.GetByIdAsync(id);
            return transporte == null ? null : _mapper.Map<RemiteTransportistaDTO>(transporte);
        }
        public async Task<int> CreateAsync(RemiteTransportistaCreateDTO createDTO)
        {
            try
            {
                var transporte = _mapper.Map<RemiteTransportistum>(createDTO);
                await _repository.CreateAsync(transporte);
                return transporte.RemiteTransportistaId;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("placa_UNIQUE")) { msg = "La placa ya se encuentra registrada."; }
                else if (msg.Contains("tarjeta_UNIQUE")) { msg = "Nro de tarjeta ya se encuentra registrado"; }
                throw new Exception(msg);
            }
        }
        public async Task<RemiteTransportistaDTO> UpdateAsync(int id, RemiteTransportistaUpdateDTO updateDTO)
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
                return _mapper.Map<RemiteTransportistaDTO>(existingTransporte);
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
                throw new KeyNotFoundException($"Unidad de Transporte con ID {id} no encontrado.");
            }
            existing.Estado = _status;
            existing.IdEstado = (sbyte?)infoRequestDto.IdEstado;
            existing.EmpleadoIdedicion = infoRequestDto.EditedEmployeeId;
            existing.EmpleadoNombreEdicion = infoRequestDto.EditedEmployeeName;
            await _repository.UpdateAsync(existing);

            return new UpdatedStatusDto
            {
                Id = existing.RemiteTransportistaId,
                IdEstado = (Int32)existing.IdEstado,
                Estado = existing.Estado,
                FechaEdicion = existing.FechaEdicion,
                EmpleadoNombreEdicion = infoRequestDto.EditedEmployeeName,
                Detalle = $"Unidad de Transporte con ID {existing.RemiteTransportistaId} ha sido actualizado a estado '{existing.Estado}'."
            };
        }
    }
}
