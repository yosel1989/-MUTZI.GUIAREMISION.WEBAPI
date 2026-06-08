using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.VentaConfig
{
    public class EmpresaDTO
    {
        [JsonPropertyName("ruc")]
        [SwaggerSchema("Devuelve en texto el ruc de la empresa que tiene acceso al facturador.", Nullable = false)]
        public string Ruc { get; set; }

        [JsonPropertyName("razon_social")]
        [SwaggerSchema("Devuelve en texto el nombre de la empresa que tiene acceso al facturador.", Nullable = false)]
        public string Empresa { get; set; }

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Devuelve en texto, la dirección fiscal de la empresa remitente", Nullable = false)]
        public string DireccionFiscal { get; set; }
    }
}
