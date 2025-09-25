using GuiaRemision.DTOs.GuiaRemitente;
using GuiaRemision.DTOs.GuiaTransportista;

namespace GuiaRemision.Services.GuiaTransportista
{
    public interface IGuiaTransportistaService
    {
        Task<GuiaTransportistaDTO> CreateAsync(GuiaTransportistaCreateDTO guiaTransportistaDto);
        Task<GuiaTransportistaDTO> GetByIdAsync(int id);
        public (bool EsValido, string Error) ValidarDatosGuiaTransportista(GuiaTransportistaCreateDTO doc);
    }
}
