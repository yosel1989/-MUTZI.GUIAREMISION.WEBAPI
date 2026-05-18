using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Area
{
    public class AreaDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("local_id")]
        public int LocalId { get; set; }

        [JsonPropertyName("estado")]
        public int Estado { get; set; }
    }
}
