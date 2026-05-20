using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.MotivoTraslado;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class MotivoTrasladoService: IMotivoTrasladoService
    {
        private readonly IAuthService _authService;
        private readonly IMotivoTrasladoRepository _repository;
        private readonly IMapper _mapper;
        public MotivoTrasladoService(
            IMotivoTrasladoRepository repository, 
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<List<MotivoTrasladoToSelectDTO>> ListToSelectAsync()
        {
            try
            {
                var data = await _repository.GetListToSelectAsync();
                return _mapper.Map<List<MotivoTrasladoToSelectDTO>>(data);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(msg);
            }
        }

    }
}
