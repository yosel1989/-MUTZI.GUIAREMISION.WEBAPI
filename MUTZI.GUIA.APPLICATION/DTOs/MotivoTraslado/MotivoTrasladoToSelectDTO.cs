using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.MotivoTraslado
{
    public class MotivoTrasladoToSelectDTO
    {
        [JsonPropertyName("id")]
        public int MotivoTrasladoId { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("Codigo")]
        public string Codigo { get; set; }
    }
}
