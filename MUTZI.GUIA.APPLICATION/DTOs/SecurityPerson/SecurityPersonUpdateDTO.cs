using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.SecurityPerson
{
    public class SecurityPersonUpdateDTO
    {
        [JsonPropertyName("id")]
        [Description("Identificador único del registro de la persona de seguridad del local.")]
        public int SecurityPersonId { get; set; }

        [JsonPropertyName("person_id")]
        [Description("Identificador único del registro de la tabla PERSONA.")]
        public int PersonId { get; set; }
    }
}
