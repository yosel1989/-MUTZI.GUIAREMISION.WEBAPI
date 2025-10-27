using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Destinatario;

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
        public async Task<DestinatarioUpdateDTO> UpdateAsync(int id, DestinatarioUpdateDTO updateDTO)
        {
            var existingDestinatario = await _repository.GetByIdAsync(id);
            if (existingDestinatario == null)
            {
                throw new KeyNotFoundException($"Destinatario con ID {id} no encontrado.");
            }
            _mapper.Map(updateDTO, existingDestinatario);
            await _repository.UpdateAsync(existingDestinatario);
            return updateDTO;
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
