using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaTransportista
{
    public interface IGuiaTransportistaService
    {
        //Task<GuiaTransportistaDTO> CreateAsync(GuiaTransportistaCreateDTO guiaTransportistaDto);
        Task<object> CreateAsync(string ruc, GuiaTransportistaCreateDTO guiaTransportistaDto);
        Task<GuiaTransportistaDTO> GetByIdAsync(int id);
        public (bool EsValido, string Error) ValidarDatosGuiaTransportista(GuiaTransportistaCreateDTO doc, string ruc);
    }
}
