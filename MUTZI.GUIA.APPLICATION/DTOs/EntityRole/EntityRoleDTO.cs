using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.EntityRole
{
    public class EntityRoleDTO
    {
        
        [JsonPropertyName("id")]
        [Description("Identificador único de la entidad")]
        public int EntityId { get; set; }

        [JsonPropertyName("role")]
        [Description("Rol de entidad: client,supplier,transporter,issuer")]
        public string Role { get; set; } = "issuer";

        [JsonPropertyName("created_at")]
        [Description("Fecha de creación del registro")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("created_at_user")]
        [Description("Usuario que creó el registro")]
        public string CreatedAtUser { get; set; }

        [JsonPropertyName("created_at_user_name")]
        [Description("Nombre del usuario que creó el registro")]
        public string CreatedAtUserName { get; set; }

    }
}
