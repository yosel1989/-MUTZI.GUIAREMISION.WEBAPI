using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.IntegrationCredential
{
    public class IntegrationCredentialUpdateDTO
    {
        [JsonPropertyName("id")]
        [Description("Identificador único de la integración.")]
        public int IntegrationCredentialId { get; set; }

        [JsonPropertyName("entity_id")]
        [Description("Identificador único de la entidad")]
        public int EntityId { get; set; }

        [JsonPropertyName("provider")]
        [Description("Proveedor del servicio a integrar")]
        public string Provider { get; set; }

        [JsonPropertyName("key")]
        [Description("Llave para servicio a integrar")]
        public string Key { get; set; }

    }
}
