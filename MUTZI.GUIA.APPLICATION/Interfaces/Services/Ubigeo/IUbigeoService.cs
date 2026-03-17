using MUTZI.GUIA.APPLICATION.DTOs.Ubigeo;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.Ubigeo
{
    public interface IUbigeoService
    {
        Task<IEnumerable<UbigeoDistritoDto>> GetDistritoAsync(string ubigeoProvincia);
        Task<IEnumerable<UbigeoProvinciaDto>> GetProvinciaAsync(string ubigeoDepartamento);
        Task<IEnumerable<UbigeoDepartamentoDto>> GetDepartamentoAsync();
        Task<UbigeoDto> FindByIdAsync(string ubigeo);
    }
}
