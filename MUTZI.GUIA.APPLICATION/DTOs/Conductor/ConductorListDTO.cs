using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Conductor
{
    public class ConductorListDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del conductor en la bbdd", Nullable = false)]
        public int ConductorId { get; set; }

        [JsonPropertyName("tipo_documento")]
        [SwaggerSchema("Devuelve un texto, el tipo de documento solo pueden tener los siguientes valores: DNI, CE y PASAPORTE", Nullable = false)]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Devuelve en texto el número de documento del conductor", Nullable = false)]
        public string NumeroDocumento { get; set; }

        [JsonPropertyName("nombres")]
        [SwaggerSchema("Devuelve en texto los nombres del conductor", Nullable = false)]
        public string Nombres { get; set; }

        [JsonPropertyName("apellidos")]
        [SwaggerSchema("Devuelve en texto los apellidos del conductor", Nullable = false)]
        public string Apellidos { get; set; }

        [JsonPropertyName("cargo")]
        [SwaggerSchema("Devuelve en texto el cargo del conductor", Nullable = true)]
        public string? Cargo { get; set; }

        [JsonPropertyName("licencia")]
        [SwaggerSchema("Devuelve el número de licencia del conductor", Nullable = false)]
        public string Licencia { get; set; }

        [JsonPropertyName("tipo")]
        [SwaggerSchema("Devuelve el tipo de conductor = INTERNO, EXTERNO", Nullable = false)]
        public string Tipo { get; set; }
    }
}
