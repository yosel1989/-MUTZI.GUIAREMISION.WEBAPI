using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Persona
{
    public class PersonaDTO
    {
        [JsonPropertyName("id")]
        public int PersonaId { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("apellido_paterno")]
        public string? ApellidoPaterno { get; set; }

        [JsonPropertyName("apellido_materno")]
        public string? ApellidoMaterno { get; set; }

        [JsonPropertyName("cargo")]
        public string? Cargo { get; set; }
    }
}
