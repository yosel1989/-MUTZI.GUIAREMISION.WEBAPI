using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace GuiaRemision.DTOs.GuiaRemitente.Conductor
{
    [SwaggerSchema("Devuelve un objeto conductor")]
    public class ConductorDTO
    {

        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del conductor en la bbdd", Nullable = false)]
        public int ConductorID { get; set; }

        [JsonPropertyName("dni")]
        [SwaggerSchema("Devuelve en texto el dni del conductor", Nullable = false)]
        public string DNI { get; set; }

        [JsonPropertyName("nombres")]
        [SwaggerSchema("Devuelve en texto los nombres del conductor", Nullable = false)]
        public string Nombres { get; set; }

        [JsonPropertyName("apellidos")]
        [SwaggerSchema("Devuelve en texto los apellidos del conductor", Nullable = false)]
        public string Apellidos { get; set; }

        [JsonPropertyName("cargo")]
        [SwaggerSchema("Devuelve en texto el cargo del conductor", Nullable = false)]
        public string Cargo { get; set; }

        [JsonPropertyName("licencia")]
        [SwaggerSchema("Devuelve el número de licencia del conductor", Nullable = false)]
        public string Licencia { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
