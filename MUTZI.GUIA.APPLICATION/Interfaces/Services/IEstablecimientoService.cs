using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IEstablecimientoService
    {
        Task<List<EstablecimientoListDTO>> GetAllByRucToSelectAsync(string ruc);
        Task<List<EstablecimientoListToModalDTO>> GetListBySuggestionAsync(string ruc, string? search);
        Task<EstablecimientoDTO?> GetByIdAsync(int id);
        Task<PagedResult<EstablecimientoDTO>> GetListAsync(int pageNumber, int pageSize, string? search);
        Task DeleteAsync(int id);
        Task<UpdatedStatusDto> CambiarEstadoAsync(int id, InfoRequestDTO infoRequestDto);
        Task<int> CreateAsync(EstablecimientoCreateDTO createDTO);

        Task<EstablecimientoDTO> UpdateAsync(int id, EstablecimientoUpdateDTO updateDTO);
    }
}
