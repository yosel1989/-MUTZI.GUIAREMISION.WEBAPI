using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.SecurityPerson
{
    public class SecurityPersonCreateDTO
    {
        [JsonPropertyName("document_number")]
        [Description("Número de documento de identidad")]
        public string DocumentNumber { get; set; }
    }
}
