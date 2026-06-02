using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.Persona;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class PersonaService: IPersonaService
    {
        private readonly IPersonaRepository _repository;
        private readonly IMapper _mapper;
        public PersonaService(
            IPersonaRepository repository, 
            IMapper mapper
        )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PersonaDTO>> GetListBySuggestion(string? text)
        {
            try
            {
                var data = await _repository.GetListBySuggestion(text);
                return _mapper.Map<List<PersonaDTO>>(data);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(msg);
            }
        }

        public async Task<PersonaDTO> GetByIdAsync(int id)
        {
            try
            {
                var data = await _repository.GetByIdAsync(id);
                if (data is null) throw new Exception("No se encontro el personal seleccionado.");

                return _mapper.Map<PersonaDTO>(data);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(msg);
            }
        }

    }
}
