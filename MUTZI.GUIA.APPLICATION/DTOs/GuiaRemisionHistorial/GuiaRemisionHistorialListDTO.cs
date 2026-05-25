using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionHistorial
{
    public class GuiaRemisionHistorialListDTO
    {
        [JsonPropertyName("motivo")]
        public string Motivo { get; set; }

        [JsonPropertyName("valor_anterior")]
        public string? ValorAnterior { get; set; }

        [JsonPropertyName("valor_nuevo")]
        public string? ValorNuevo { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("fecha_registro")]
        public DateTime FechaRegistro { get; set; }

        [JsonPropertyName("usuario_registro")]
        public string UsuarioRegistro { get; set; }

        [JsonPropertyName("usuario_registro_nombre")]
        public string UsuarioRegistroNombre { get; set; }

        [JsonPropertyName("area")]
        public string AreaNombre { get; set; } = string.Empty;

    }
}
