using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Conductor
{
    [SwaggerSchema("Devuelve un objeto conductor asociado a los datos de envío de una guía de remisión")]
    public class ConductorGuiaDTO
    {
        [JsonPropertyName("conductor_guia_id")]
        [SwaggerSchema("Devuelve en entero el ID del conductor asociado a los datos de envío de una guía de remisión.", Nullable = false)]
        public int GuiaRemisionConductorId { get; set; }

        [JsonPropertyName("conductor_id")]
        [SwaggerSchema("Devuelve en entero el ID del conductor en la base de datos", Nullable = false)]
        public int ConductorId { get; set; }

        [JsonPropertyName("datos_envio_id")]
        [SwaggerSchema("Devuelve en entero el ID de los datos de envío asociados a la guía de remisión", Nullable = false)]
        public int GuiaRemisionDatoEnvioId { get; set; }

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

        [JsonPropertyName("job_title")]
        [SwaggerSchema("Devuelve un orden de conductor de Principal a Secundario. Ref.: El 1ero de la lista es: 'Principal', el resto de la lista es: 'Secundario'.")]
        public string JobTitle { get; set; }
    }
}
