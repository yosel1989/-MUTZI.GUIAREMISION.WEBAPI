using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente
{
    public interface IGuiaRemitenteService
    {
        Task<List<GuiaRemitenteDTO>> GetAllAsync();
        Task<GuiaRemitenteDTO> GetByIdAsync(int id);
        Task<GuiaPorTicketDTO> GetByTicketAsync(string ticket);
        Task<GuiaRemitenteDTO> CreateAsync(GuiaRemitenteCreateDTO guiaRemisionDto);
        Task<GuiaRemitenteUpdateDTO> UpdateAsync(int id, GuiaRemitenteUpdateDTO guiaRemisionDto);
        Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estado);
        public (bool EsValido, string Error) ValidarDatosGuiaRemitente(GuiaRemitenteCreateDTO doc);
        Task DeleteAsync(int id);
    }
}
