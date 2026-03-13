using MUTZI.GUIA.APPLICATION.DTOs.Ubigeo;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.Ubigeo;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Ubigeo;

namespace MUTZI.GUIA.APPLICATION.Services.Ubigeo
{
    public class UbigeoService: IUbigeoService
    {
        private readonly IUbigeoRepository _repository;
        public UbigeoService(IUbigeoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UbigeoDistritoDto>> GetDistritoAsync(string ubigeoProvincia)
        {
            return await _repository.GetDistritoAsync(ubigeoProvincia);
        }
        public async Task<IEnumerable<UbigeoProvinciaDto>> GetProvinciaAsync(string ubigeoDepartamento)
        {
            return await _repository.GetProvinciaAsync(ubigeoDepartamento);
        }
        public async Task<IEnumerable<UbigeoDepartamentoDto>> GetDepartamentoAsync()
        {
            try
            {
                return await _repository.GetDepartamentoAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<UbigeoDto> FindByIdAsync(string ubigeo)
        {
            return await _repository.FindByIdAsync(ubigeo);
        }
    }
}
