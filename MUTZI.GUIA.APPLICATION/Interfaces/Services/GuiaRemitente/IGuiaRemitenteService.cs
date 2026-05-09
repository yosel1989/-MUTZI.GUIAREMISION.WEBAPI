using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente
{
    public interface IGuiaRemitenteService
    {
        Task<List<GuiaRemitenteDTO>> GetAllAsync();
        Task<PagedResult<GuiaRemisionListDTO>> GetListAsync(int pageNumber, int pageSize, IQueryCollection q);
        Task<byte[]> ExportGuiasAsync(int pageNumber, int pageSize, IQueryCollection q);
        Task<object> GetByIdAsync(int id);
        Task<GuiaPorTicketDTO> GetByTicketAsync(string ticket);
        Task<object> CreateAsync(string ruc, GuiaRemitenteCreateDTO guiaRemisionDto);
        Task<GuiaRemitenteDTO> UpdateAsync(int id, GuiaRemitenteUpdateDTO guiaRemisionDto);
        Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estado, int idEstado);
        (bool EsValido, string Error) ValidarDatosGuiaRemitente(GuiaRemitenteCreateDTO doc, string ruc, Establecimiento? remitente, Establecimiento? destinatario, VentaConfig? ventaConfig);
        (bool EsValido, string Error) ValidarDatosGuiaRemitente(GuiaRemitenteDTO doc, string ruc, Establecimiento? remitente, Establecimiento? destinatario, VentaConfig? ventaConfig);
        Task DeleteAsync(int id);

        Task<GuiaRemisionListDTO> ConfirmarAsync(int id);
    }
}
