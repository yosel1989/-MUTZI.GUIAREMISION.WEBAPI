using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.SecurityPerson
{
    public class SecurityPersonUpdateDTO
    {
        [JsonPropertyName("id")]
        [Description("Identificador único del registro de la persona de seguridad del local.")]
        public int SecurityPersonId { get; set; }

        [JsonPropertyName("document_number")]
        [Description("Número de documento de identidad")]
        public string DocumentNumber { get; set; }
    }
}
