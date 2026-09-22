using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.SecurityPerson
{
    public class SecurityPersonCreateDTO
    {
        [JsonPropertyName("person_id")]
        [Description("Identificador único del registro de la tabla PERSONA.")]
        public int PersonId { get; set; }
    }
}
