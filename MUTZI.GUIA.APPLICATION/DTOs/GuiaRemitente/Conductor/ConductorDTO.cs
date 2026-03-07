using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Conductor
{
    [SwaggerSchema("Devuelve un objeto conductor")]
    public class ConductorDTO
    {

        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del conductor en la bbdd", Nullable = false)]
        public int ConductorID { get; set; }

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

        [JsonPropertyName("fecha_creacion")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00", Nullable = false)]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("empleado_nombre_creacion")]
        [SwaggerSchema("Devuelve un texto, el nombre del empleado que ha registrado la transacción en el sistema", Nullable = false)]
        public string EmpleadoNombreCreacion { get; set; }

        [JsonPropertyName("fecha_ultima_edicion")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00, es la fecha de la última edición realizada en la guía", Nullable = true)]
        public DateTime? FechaEdicion { get; set; }

        [JsonPropertyName("empleado_nombre_edicion")]
        [SwaggerSchema("Devuelve en texto el nombre del empleado que ha realizado la última edición en la guía", Nullable = true)]
        public string? EmpleadoNombreEdicion { get; set; }

        [JsonPropertyName("estado")]
        [SwaggerSchema("Devuelve un texto, el estado del conductor solo puede ser: ACTIVO o INACTIVO", Nullable = false)]
        public string Estado { get; set; }

        [JsonPropertyName("id_estado")]
        [SwaggerSchema("Devuelve un entero, el ID del estado del conductor en la bbdd", Nullable = false)]
        public int IdEstado { get; set; }
    }
}
