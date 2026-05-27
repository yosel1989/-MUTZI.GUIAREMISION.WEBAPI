using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.TipoDocumento;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class TipoDocumentoService: ITipoDocumentoService
    {
        private readonly IAuthService _authService;
        private readonly ITipoDocumentoRepository _repository;
        private readonly IMapper _mapper;
        public TipoDocumentoService(
            ITipoDocumentoRepository repository, 
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<List<TipoDocumentoDTO>> ListToSelectAsync()
        {
            try
            {
                var data = await _repository.GetListToSelectAsync();
                return _mapper.Map<List<TipoDocumentoDTO>>(data);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(msg);
            }
        }

    }
}
