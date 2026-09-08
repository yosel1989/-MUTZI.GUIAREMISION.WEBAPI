using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.IntegrationCredential
{
    public class IntegrationCredentialDTO
    {
        [JsonPropertyName("id")]
        [Description("Identificador único de la integración.")]
        public int IntegrationCredentialId { get; set; }

        [JsonPropertyName("entity_id")]
        [Description("Identificador único de la entidad")]
        public int? EntityId { get; set; }

        [JsonPropertyName("entity_name")]
        [Description("Nombre de la entidad")]
        public string? EntityName { get; set; }

        [JsonPropertyName("entity_document_number")]
        [Description("Número del documento de la entidad")]
        public string? EntityDocumentNumber { get; set; }

        [JsonPropertyName("provider")]
        [Description("Proveedor del servicio a integrar")]
        public string Provider { get; set; }

        [JsonPropertyName("key")]
        [Description("Llave para servicio a integrar")]
        public string Key { get; set; }

        [JsonPropertyName("key_test")]
        [Description("Llave de prueba para servicio a integrar")]
        public string? KeyTest { get; set; }

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
