using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.UnidadMedida;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class UnidadMedidaService: IUnidadMedidaService
    {
        private readonly IUnidadMedidaRepository _repository;
        private readonly IMapper _mapper;
        public UnidadMedidaService(
            IUnidadMedidaRepository repository, 
            IMapper mapper
        )
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<UnidadMedidaToSelectDTO>> ListToSelectAsync()
        {
            try
            {
                var data = await _repository.GetListToSelectAsync();
                return _mapper.Map<List<UnidadMedidaToSelectDTO>>(data);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(msg);
            }
        }

        public async Task<List<UnidadMedidaToSelectDTO>> ListPesoToSelectAsync()
        {
            try
            {
                var data = await _repository.GetListPesoToSelectAsync();
                return _mapper.Map<List<UnidadMedidaToSelectDTO>>(data);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(msg);
            }
        }

    }
}
