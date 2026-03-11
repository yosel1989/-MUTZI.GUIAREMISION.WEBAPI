using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
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
        public async Task<PagedResult<DestinatarioDTO>> GetListAsync(int pageNumber, int pageSize)
        {
            var(destinatario, totalRecords)=await _repository.GetListAsync(pageNumber, pageSize);
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
        public async Task<int> CreateAsync(DestinatarioCreateDTO createDTO)
        {
            var destinatario = _mapper.Map<DOMAIN.Entities.Destinatario>(createDTO);
            await _repository.CreateAsync(destinatario);
            return destinatario.DestinatarioId;
        }
        public async Task<DestinatarioDTO> UpdateAsync(int id, DestinatarioUpdateDTO updateDTO)
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
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task<UpdatedStatusDto> CancelAsync(int id, int status)
        {
            string _status = status switch
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
            existing.IdEstado = (sbyte?)status;
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
