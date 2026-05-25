using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.MotivoTraslado
{
    public class MotivoTrasladoDTO
    {
        [JsonPropertyName("id")]
        public int MotivoTrasladoId { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }
    }
}
