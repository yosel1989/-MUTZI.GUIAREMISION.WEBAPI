using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs
{
    public class ResponseDTO<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("detalle")]
        public string Detalle { get; set; }
    }
}