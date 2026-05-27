using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Shared
{
    public class EstadoActualizadoRequestDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Ingresa un entero, el Id del registro afectado.", Nullable = false)]
        public int Id { get; set; }

        [JsonPropertyName("id_estado")]
        [SwaggerSchema("Ingresa en entero el nuevo estado del registro.", Nullable = false)]
        public int EstadoId { get; set; }
    }

    public class EstadoActualizadoResponseDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID afectado por la transacción en la bbdd.", Nullable = false)]
        public int Id { get; set; }

        [JsonPropertyName("id_estado")]
        [SwaggerSchema("Devuelve en entero el nuevo estado del registro.", Nullable = false)]
        public int EstadoId { get; set; }

        [JsonPropertyName("estado")]
        [SwaggerSchema("Devuelve el nuevo estado del registro.", Nullable = false)]
        public string Estado { get; set; }

        [JsonPropertyName("fecha_modifico")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00, es la fecha de la última edición realizada en la guía", Nullable = true)]
        public DateTime? FechaModifico { get; set; }

        [JsonPropertyName("usuario_modifico")]
        [SwaggerSchema("Recibe un texto, se coloca el usuario que modifico el estado del registro.", Nullable = false)]
        public string? UsuarioModifico { get; set; }

        [JsonPropertyName("usuario_modifico_nombre")]
        [SwaggerSchema("Recibe un texto, se coloca el nombre del usuario que modifico el estado del registro.", Nullable = false)]
        public string? UsuarioModificoNombre { get; set; }

    }
}
