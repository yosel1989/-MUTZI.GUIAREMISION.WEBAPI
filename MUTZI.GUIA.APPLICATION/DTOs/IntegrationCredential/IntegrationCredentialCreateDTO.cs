using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.IntegrationCredential
{
    public class IntegrationCredentialCreateDTO
    {

        [JsonPropertyName("entity_id")]
        [Description("Identificador único de la entidad")]
        public int? EntityId { get; set; }

        [JsonPropertyName("provider")]
        [Description("Proveedor del servicio a integrar")]
        public string Provider { get; set; }

        [JsonPropertyName("key")]
        [Description("Llave para servicio a integrar")]
        public string Key { get; set; }

        [JsonPropertyName("key_test")]
        [Description("Llave de prueba para servicio a integrar")]
        public string? KeyTest { get; set; }

    }
}
