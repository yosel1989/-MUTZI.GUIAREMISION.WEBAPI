using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.EntityRole
{
    public class EntityRoleToSyncToEntityDTO
    {
        [JsonPropertyName("id")]
        [Description("Etiqueta del rol")]
        public int EntityId { get; set; }

        [JsonPropertyName("roles")]
        [Description("Listado de roles a sincronizar")]
        public List<string> Roles { get; set; }

    }
}
