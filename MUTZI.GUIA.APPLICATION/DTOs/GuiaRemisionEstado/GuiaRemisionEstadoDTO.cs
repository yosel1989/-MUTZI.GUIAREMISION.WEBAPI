using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionEstado
{
    public class GuiaRemisionEstadoDTO
    {
        [JsonPropertyName("id")]
        public int GuiaRemisionEstadoId { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
    }
}
