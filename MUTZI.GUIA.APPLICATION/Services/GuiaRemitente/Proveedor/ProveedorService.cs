using AutoMapper;
using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Proveedor;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Services.GuiaRemitente.Proveedor
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _repository;
        private readonly IMapper _mapper;
        public ProveedorService(IProveedorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<ProveedorDTO>> GetAllAsync()
        {
            var proveedores = await _repository.GetAllAsync();
            return _mapper.Map<List<ProveedorDTO>>(proveedores);
        }
        public async Task<List<ProveedorListDTO>> GetListBySuggestion(string numeroDoc)
        {
            var proveedores = await _repository.GetListBySuggestion(numeroDoc);
            return _mapper.Map<List<ProveedorListDTO>>(proveedores);
        }
        public async Task<PagedResult<ProveedorDTO>> GetListAsync(int pageNumber, int pageSize, IQueryCollection q)
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
            var (proveedor, totalRecords)=await _repository.GetListAsync(pageNumber, pageSize, request);
            var proveedorDTO = _mapper.Map<List<ProveedorDTO>>(proveedor);
            return new PagedResult<ProveedorDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = proveedorDTO
            };
        }

        public async Task<ProveedorDTO?> GetByIdAsync(int id)
        {
            var proveedor = await _repository.GetByIdAsync(id);
            return proveedor == null ? null : _mapper.Map<ProveedorDTO>(proveedor);
        }

        public async Task<ProveedorDTO?> GetByNumeroDocumentoAsync(string numeroDocumento)
        {
            var proveedor = await _repository.GetByNumeroDocumentoAsync(numeroDocumento);
            return proveedor == null ? null : _mapper.Map<ProveedorDTO>(proveedor);
        }

        public async Task<int> CreateAsync(ProveedorCreateDTO createDTO)
        {
            try
            {
                var proveedor = _mapper.Map<DOMAIN.Entities.Proveedor>(createDTO);
                await _repository.CreateAsync(proveedor);
                return proveedor.ProveedorId;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("numeroDocumento_UNIQUE")) { msg = "El número de documento ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }
        public async Task<ProveedorDTO> UpdateAsync(int id, ProveedorUpdateDTO updateDTO)
        {
            try
            {
                var existingProveedor = await _repository.GetByIdAsync(id);
                if (existingProveedor == null)
                {
                    throw new KeyNotFoundException($"Proveedor con ID {id} no encontrado.");
                }
                _mapper.Map(updateDTO, existingProveedor);
                await _repository.UpdateAsync(existingProveedor);
                return _mapper.Map<ProveedorDTO>(existingProveedor);
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
            var existing=await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Proveedor con ID {id} no encontrado.");
            }
            existing.Estado = _status;
            existing.IdEstado = (sbyte?)infoRequestDto.IdEstado;
            existing.EmpleadoIdedicion = infoRequestDto.EditedEmployeeId;
            existing.EmpleadoNombreEdicion = infoRequestDto.EditedEmployeeName;
            await _repository.UpdateAsync(existing);

            return new UpdatedStatusDto
            {
                Id = existing.ProveedorId,
                IdEstado = Convert.ToInt32(existing.IdEstado),
                Estado = existing.Estado,
                Detalle = $"Proveedor con ID {existing.ProveedorId} ha sido actualizado a estado '{existing.Estado}'."
            };
        }
    }
}
