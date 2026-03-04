using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.EmailRemiteTransportistaGuia;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.RemiteTransportista
{
    public class RemiteTransportistaDTO
    {
        [JsonPropertyName("tipo_documento")]
        [SwaggerSchema("Devuelve en texto el tipo de documento del REMITE_TRANSPORTISTA, solo pueden tener los siguientes valores: DNI, CE, RUC y PASAPORTE", Nullable = false)]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Devuelve en texto el número documento del REMITE_TRANSPORTISTA.", Nullable = false)]
        public string NumeroDocumento { get; set; }

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
