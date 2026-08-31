using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.PersonaEmpresaInfo
{
    public class CompanyInfoDTO
    {
        [JsonPropertyName("document_number")]
        public string DocumentNumber { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("department")]
        public string? Department { get; set; }

        [JsonPropertyName("province")]
        public string? Province { get; set; }

        [JsonPropertyName("district")]
        public string? District { get; set; }
    }
}
