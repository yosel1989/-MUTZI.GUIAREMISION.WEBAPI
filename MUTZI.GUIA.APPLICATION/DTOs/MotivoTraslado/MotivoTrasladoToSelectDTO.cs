using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.MotivoTraslado
{
    public class MotivoTrasladoToSelectDTO
    {
        [JsonPropertyName("id")]
        public int MotivoTrasladoId { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("codigo_sunat")]
        public string CodigoSunat { get; set; }
    }
}
