namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.Ubigeo
{
    public interface IUbigeoRepository
    {
        Task<string> GetDistritoAsync(string ubigeo);
        Task<string> GetProvinciaAsync(string ubigeo);
        Task<string> GetDepartamentoAsync(string ubigeo);
    }
}
