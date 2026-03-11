using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
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
        public async Task<List<DestinatarioDTO>> GetListByName(string numeroDoc, string nombre)
        {
            var destinatarios=await _repository.GetListByName(numeroDoc, nombre);
            return _mapper.Map<List<DestinatarioDTO>>(destinatarios);
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
        public async Task CancelAsync(int id, int status)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Destinatario con ID {id} no encontrado.");
            }
            existing.Estado = status == 1 ? "activo" : "inactivo";
            existing.IdEstado = (sbyte?)status;
            await _repository.UpdateAsync(existing);
        }
    }
}
