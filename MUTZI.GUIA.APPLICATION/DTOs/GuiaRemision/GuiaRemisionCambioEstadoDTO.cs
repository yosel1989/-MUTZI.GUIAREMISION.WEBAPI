using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemision
{
    public class GuiaRemisionCambioEstadoDTO
    {
        [JsonPropertyName("id")]
        public int GuiaRemisionId { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
    }
}
