using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.OperacionResponsable
{
    public class OperacionResponsableDTO
    {
        [JsonPropertyName("id")]
        public int OperacionResponsableId { get; set; }

        [JsonPropertyName("guia_remision_estado_id")]
        public int GuiaRemisionEstadoId { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("personal_registro_id")]
        public int PersonalRegistroId { get; set; }

        [JsonPropertyName("usuario_registro")]
        public string UsuarioRegistro { get; set; }

        [JsonPropertyName("usuario_registro_nombre")]
        public string UsuarioRegistroNombre { get; set; }

        [JsonPropertyName("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("personal_modifico_id")]
        public int? PersonalModificoId { get; set; }

        [JsonPropertyName("usuario_modifico")]
        public string? UsuarioModifico { get; set; }

        [JsonPropertyName("usuario_modifico_nombre")]
        public string? UsuarioModificoNombre { get; set; }

        [JsonPropertyName("fecha_modifico")]
        public DateTime? FechaModifico { get; set; }
    }
}
