using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.Area;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class AreaService: IAreaService
    {
        private readonly IAuthService _authService;
        private readonly IAreaRepository _repository;
        private readonly IMapper _mapper;
        public AreaService(
            IAreaRepository repository, 
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<List<AreaDTO>> ListToSelectAsync()
        {
            try
            {
                var data = await _repository.GetListToSelectAsync();
                return _mapper.Map<List<AreaDTO>>(data);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(msg);
            }
        }

        public async Task<IEnumerable<AreaDTO>> GetListBySuggestion(string? text)
        {
            try
            {
                var data = await _repository.GetListBySuggestion(text);
                return _mapper.Map<List<AreaDTO>>(data);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(msg);
            }
        }

    }
}
