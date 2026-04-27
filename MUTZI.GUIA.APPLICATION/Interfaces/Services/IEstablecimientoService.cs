using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IEstablecimientoService
    {
        Task<List<EstablecimientoListDTO>> GetAllByRucToSelectAsync(string ruc);
        Task<List<EstablecimientoListToModalDTO>> GetListBySuggestionAsync(string ruc, string? search);
        Task<EstablecimientoDTO?> GetByIdAsync(int id);
    }
}
