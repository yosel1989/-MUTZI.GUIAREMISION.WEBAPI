using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Perfil
{
    public class PerfilDTO
    {
        [JsonPropertyName("id")]
        public int PerfilId { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = null!;

        [JsonPropertyName("Descripcion")]
        public string? Descripcion { get; set; } = null!;

        [JsonPropertyName("Codigo")]
        public int Codigo { get; set; }

        [JsonPropertyName("estado")]
        public string Estado { get; set; } = "activo";

        [JsonPropertyName("id_estado")]
        public int EstadoId { get; set; } = 1;

    

        [JsonPropertyName("fecha_registro")]
        public DateTime FechaRegistro { get; set; }

        [JsonPropertyName("fecha_modifico")]
        public DateTime? FechaModifico { get; set; }

        [JsonPropertyName("usuario_registro")]
        public string UsuarioRegistro { get; set; }

        [JsonPropertyName("usuario_modifico")]
        public string? UsuarioModifico { get; set; }
    }
}
