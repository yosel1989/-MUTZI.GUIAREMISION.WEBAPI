using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.EntityRole
{
    public class EntityRoleCreateDTO
    {

        [JsonPropertyName("entity_id")]
        [Description("Identificador único de la entidad")]
        public int EntityId { get; set; }

        [JsonPropertyName("role")]
        [Description("Rol de entidad: client,supplier,transporter,issuer")]
        public string Role { get; set; } = "issuer";

    }
}
