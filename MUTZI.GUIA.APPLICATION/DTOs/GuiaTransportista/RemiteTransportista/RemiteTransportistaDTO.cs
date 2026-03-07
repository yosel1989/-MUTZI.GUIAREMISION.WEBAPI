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

        [JsonPropertyName("codigo_sunat")]
        [SwaggerSchema("Devuelve en texto el Código de establecimiento o de la Sucursal", Nullable = true)]
        public string? CodigoSunat { get; set; }

        [JsonPropertyName("email_remite_transportista")]
        public List<EmailRemiteTransportistaGuiaDTO>? EmailRemiteTransportista { get; set; }

        [JsonPropertyName("estado")]
        [SwaggerSchema("Devuelve un texto, el estado del REMITE_TRANSPORTISTA solo puede ser: ACTIVO o INACTIVO", Nullable = false)]
        public string Estado { get; set; }

        [JsonPropertyName("id_estado")]
        [SwaggerSchema("Devuelve un entero, el ID del estado del REMITE_TRANSPORTISTA en la bbdd", Nullable = false)]
        public int IdEstado { get; set; }
    }
}
