using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.VentaConfig
{
    public class EmpresaListDTO
    {
        [JsonPropertyName("ruc")]
        [SwaggerSchema("Devuelve en texto el ruc de la empresa que tiene acceso al facturador.", Nullable = false)]
        public string Ruc { get; set; }

        [JsonPropertyName("empresa")]
        [SwaggerSchema("Devuelve en texto el nombre de la empresa que tiene acceso al facturador.", Nullable = false)]
        public string Empresa { get; set; }
    }
}
