using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Catalogos
{
    public partial class CountryDTO
    {
        [JsonPropertyName("id")]
        public int CountryId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}
