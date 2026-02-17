using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.EmailRemiteTransportistaGuia;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.RemiteTransportista
{
    public class RemiteTransportistaDTO
    {
        public string Ruc { get; set; }

        public string RazonSocial { get; set; }

        public string UbigeoID { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Direccion { get; set; }
        //public string Email { get; set; }
        public string Pais { get; set; }

        [JsonPropertyName("email_remite_transportista")]
        public List<EmailRemiteTransportistaGuiaDTO>? EmailRemiteTransportista { get; set; }
    }
}
