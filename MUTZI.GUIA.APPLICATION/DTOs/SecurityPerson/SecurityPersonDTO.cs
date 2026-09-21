using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.SecurityPerson
{
    public class SecurityPersonDTO
    {
        [JsonPropertyName("id")]
        [Description("Identificador único del registro de la persona de seguridad del local.")]
        public int SecurityPersonId { get; set; }

        [JsonPropertyName("document_number")]
        [Description("Número de documento de identidad")]
        public string DocumentNumber { get; set; }

        [JsonPropertyName("full_name")]
        [Description("Número de documento de identidad")]
        public string PersonFullName { get; set; }

        [JsonPropertyName("role")]
        [Description("Cargo de la persona en la empresa.")]
        public string Role { get; set; }

        [JsonPropertyName("active")]
        [Description("Estado de la entidad: activa o inactiva")]
        public bool Active { get; set; } = true;

        [JsonPropertyName("created_at")]
        [Description("Fecha de creación del registro")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("created_at_user")]
        [Description("Usuario que creó el registro")]
        public string CreatedAtUser { get; set; }

        [JsonPropertyName("created_at_user_name")]
        [Description("Nombre del usuario que creó el registro")]
        public string CreatedAtUserName { get; set; }

        [JsonPropertyName("updated_at")]
        [Description("Fecha de última modificación")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("updated_at_user")]
        [Description("Usuario que modificó el registro")]
        public string? UpdatedAtUser { get; set; }

        [JsonPropertyName("updated_at_user_name")]
        [Description("Nombre del usuario que modificó el registro")]
        public string? UpdatedAtUserName { get; set; }
    }
}
