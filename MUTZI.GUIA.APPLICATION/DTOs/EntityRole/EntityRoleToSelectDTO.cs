using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.EntityRole
{
    public class EntityRoleToSelectDTO
    {
        [JsonPropertyName("label")]
        [Description("Etiqueta del rol")]
        public string Label { get; set; }

        [JsonPropertyName("role")]
        [Description("Rol de entidad: client,supplier,transporter,issuer")]
        public string Role { get; set; }

    }
}
