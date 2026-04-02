using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente
{
    public interface IGuiaRemitenteService
    {
        Task<List<GuiaRemitenteDTO>> GetAllAsync();
        Task<PagedResult<GuiaListDTO>> GetListAsync(int pageNumber, int pageSize, IQueryCollection q);
        Task<byte[]> ExportGuiasAsync(int pageNumber, int pageSize, IQueryCollection q);
        Task<object> GetByIdAsync(int id);
        Task<GuiaPorTicketDTO> GetByTicketAsync(string ticket);
        Task<GuiaRemitenteDTO> CreateAsync(GuiaRemitenteCreateDTO guiaRemisionDto);
        Task<GuiaRemitenteDTO> UpdateAsync(int id, GuiaRemitenteUpdateDTO guiaRemisionDto);
        Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estado, int idEstado);
        public (bool EsValido, string Error) ValidarDatosGuiaRemitente(GuiaRemitenteCreateDTO doc, string ruc);
        Task DeleteAsync(int id);
    }
}
