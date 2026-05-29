using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Conductor
{
    [SwaggerSchema("Devuelve un objeto conductor")]
    public class ConductorDTO
    {

        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del conductor en la bbdd", Nullable = false)]
        public int ConductorId { get; set; }

        [JsonPropertyName("tipo_documento_id")]
        [SwaggerSchema("Devuelve un numero, el tipo de documento solo puede ser uno registrado en la tabla catalogo", Nullable = false)]
        public int TipoDocumentoId { get; set; }

        [JsonPropertyName("tipo_documento")]
        [SwaggerSchema("Devuelve un texto, el nombre del tipo de documento", Nullable = false)]
        public string TipoDocumentoNombre { get; set; }

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

        [JsonPropertyName("fecha_registro")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00", Nullable = false)]
        public DateTime FechaRegistro { get; set; }

        [JsonPropertyName("usuario_registro")]
        [SwaggerSchema("Devuelve un texto, el usuario que registro el conductor", Nullable = false)]
        public string UsuarioRegistro { get; set; }

        [JsonPropertyName("usuario_registro_nombre")]
        [SwaggerSchema("Devuelve un texto, el nombre del usuario que registro el conductor", Nullable = false)]
        public string UsuarioRegistroNombre { get; set; }

        [JsonPropertyName("fecha_modifico")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00, es la fecha en que se modifico el registro", Nullable = true)]
        public DateTime? FechaModifico { get; set; }

        [JsonPropertyName("usuario_modifico")]
        [SwaggerSchema("Devuelve en texto el usuario que modifico el registro.", Nullable = true)]
        public string? UsuarioModifico { get; set; }

        [JsonPropertyName("usuario_modifico_nombre")]
        [SwaggerSchema("Devuelve en texto el nombre del usuario que modifico el registro.", Nullable = true)]
        public string? UsuarioModificoNombre { get; set; }

        [JsonPropertyName("estado")]
        [SwaggerSchema("Devuelve un texto, el estado del conductor solo puede ser: activo o inactivo", Nullable = false)]
        public string Estado { get; set; }

        [JsonPropertyName("id_estado")]
        [SwaggerSchema("Devuelve un entero, el ID del estado del conductor en la bbdd", Nullable = false)]
        public int EstadoId { get; set; }

        [JsonPropertyName("tipo")]
        [SwaggerSchema("Devuelve un texto, el tipo de conducto solo puede ser: interno o externo", Nullable = false)]
        public string Tipo { get; set; }
    }
}
