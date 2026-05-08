using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaTransportista
{
    public interface IGuiaTransportistaService
    {
        Task<object> CreateAsync(string ruc, GuiaTransportistaCreateDTO guiaTransportistaDto);
        Task<GuiaTransportistaDTO> UpdateAsync(int id, GuiaTransportistaUpdateDTO guiaTransportistaDto);
        Task<GuiaTransportistaDTO> GetByIdAsync(int id);
        public (bool EsValido, string Error) ValidarDatosGuiaTransportista(GuiaTransportistaCreateDTO doc, string ruc, Establecimiento remitente, Establecimiento destinatario, Transportista remiteTransportista, VentaConfig ventaConfig);
        public (bool EsValido, string Error) ValidarDatosGuiaTransportista(GuiaTransportistaDTO doc, string ruc, Establecimiento remitente, Establecimiento destinatario, Transportista remiteTransportista, VentaConfig ventaConfig);
    }
}
