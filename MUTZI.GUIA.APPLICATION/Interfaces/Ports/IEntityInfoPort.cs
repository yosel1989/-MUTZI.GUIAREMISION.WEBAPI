using MUTZI.GUIA.DOMAIN.Models;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Ports
{
    public interface IEntityInfoPort
    {
        Task<PersonInfo> GetPersonInfoAsync(string documentNumber);
        Task<CompanyInfo> GetCompanyInfoAsync(string documentNumber);
    }
}
