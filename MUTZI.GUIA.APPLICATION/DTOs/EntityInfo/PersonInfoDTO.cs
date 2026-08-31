using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.PersonaEmpresaInfo
{
    public class PersonInfoDTO
    {
        [JsonPropertyName("document_number")]
        public string DocumentNumber { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("birth_date")]
        public DateOnly? BirthDate { get; set; }

        [JsonPropertyName("gender")]
        public string? Gender { get; set; }
    }
}
