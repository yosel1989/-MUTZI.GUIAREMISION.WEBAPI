using MUTZI.GUIA.APPLICATION.DTOs.PersonaEmpresaInfo;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IEntityInfoService
    {
        Task<PersonInfoDTO> GetPersonInfoAsync(string documentNumber);
        Task<CompanyInfoDTO> GetCompanyInfoAsync(string documentNumber);
    }
}
