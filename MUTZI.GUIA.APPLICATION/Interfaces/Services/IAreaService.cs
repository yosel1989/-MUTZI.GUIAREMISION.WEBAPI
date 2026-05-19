using MUTZI.GUIA.APPLICATION.DTOs.Area;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IAreaService
    {
        Task<List<AreaDTO>> ListToSelectAsync();
        Task<IEnumerable<AreaDTO>> GetListBySuggestion(string? text);
    }
}
