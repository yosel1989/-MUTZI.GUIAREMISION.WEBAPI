using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class GuiaRemisionIndicadorTrasladoService: IGuiaRemisionIndicadorTrasladoService
    {
        private readonly IGuiaRemisionIndicadorTrasladoRepository _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public GuiaRemisionIndicadorTrasladoService(
            IGuiaRemisionIndicadorTrasladoRepository repository, 
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<List<GuiaRemisionIndicadorTrasladoToSelectDTO>> GetToSelectAsync()
        {
            var indicadores = await _repository.GetListToSelectAsync();
            return _mapper.Map<List<GuiaRemisionIndicadorTrasladoToSelectDTO>>(indicadores);
        }

    }
}
